using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Controlador geral de áudio com suporte a sliders dinâmicos.
/// Agora os sliders são localizados AUTOMATICAMENTE na cena se as
/// referências em Inspector estiverem vazias, permitindo ter UI de volume
/// em qualquer cena sem precisar configurar manualmente.
/// </summary>
public class Controlador_Som : MonoBehaviour
{
    public static Controlador_Som instancia;

    #region Referências (Inspector ou Automáticas)
    [Header("Mixer e UI")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderMusica;  // Se vazio, será buscado por nome "BGMVolume"
    [SerializeField] private Slider sliderSFX;     // Se vazio, busca "SFXVolume"
    [SerializeField] private Slider sliderMaster;  // Se vazio, busca "MasterVolume"

    [Header("Audio Source Principal")]
    [SerializeField] private AudioSource musicaSource; // loop=true, playOnAwake=false

    [Header("BGMs – Gerais")]
    [SerializeField] private AudioClip musicaMenu;
    [SerializeField] private AudioClip musicaComemoracao;
    [SerializeField] private AudioClip musicaFalha;

    [Header("BGMs – Fases")]
    [SerializeField] private AudioClip musicaTutorial;
    [SerializeField] private AudioClip musicaTatu;
    [SerializeField] private AudioClip musicaAlien;
    [SerializeField] private AudioClip musicaDino;

    [Header("Mute (Opcional)")]
    [SerializeField] private Button muteButton;
    [SerializeField] private Sprite somLigadoSprite;
    [SerializeField] private Sprite somDesligadoSprite;
    #endregion

    private bool estaMutado;

    /*------------------------------------------------------------------*/
    #region Singleton
    private void Awake()
    {
        if (instancia != null && instancia != this) { Destroy(gameObject); return; }
        instancia = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;
    #endregion

    /*------------------------------------------------------------------*/
    #region Init + Update
    private void Start()
    {
        RestaurarVolumes();
        TrocarMusicaPorCena(SceneManager.GetActiveScene().name);
        InvokeRepeating(nameof(SincronizarSliders), 0.5f, 0.5f); // tenta sincronizar várias vezes
    }

    private void Update()
    {
        if (musicaSource && musicaSource.isPlaying && musicaSource.clip == musicaMenu)
            PlayerPrefs.SetFloat("MusicaTempo", musicaSource.time);
    }
    #endregion

    /*------------------------------------------------------------------*/
    #region Cena Carregada
    private void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        TrocarMusicaPorCena(cena.name);
        SincronizarSliders();
    }
    #endregion

    /*------------------------------------------------------------------*/
    #region Troca Automática de Música
    private void TrocarMusicaPorCena(string nome)
    {
        AudioClip alvo = musicaMenu;
        bool deveTocar = true;
        bool deveRepetir = true;

        switch (nome)
        {
            case "Tutorial": alvo = musicaTutorial; break;
            case "Fase_TatuMafioso_01": alvo = musicaTatu; break;
            case "Fase_Alien_02": alvo = musicaAlien; break;
            case "Fase_Dino_03": alvo = musicaDino; break;

            case "CenaFalhaTatu":
            case "CenaFalhaAlien":
            case "CenaFalhaDino":
                alvo = musicaFalha;
                deveRepetir = false;
                break;

            case "CenaFimDeFaseTatu":
            case "CenaFimDeFaseAlien":
            case "CenaFimDeFaseDino":
                alvo = musicaComemoracao;
                deveRepetir = false;
                break;

            case "MenuPrincipal":
            case "Creditos":
            case "CenaSelecaoFase":
                alvo = musicaMenu;
                break;

            default:
                deveTocar = false; // não toca nada se a cena não for reconhecida
                break;
        }

        if (deveTocar)
            TocarMusica(alvo, deveRepetir);
    }


    #endregion

    /*------------------------------------------------------------------*/
    #region API – Música fim de fase
    public void DefinirMusicaFimDeFase(bool falhou)
        => TocarMusica(falhou ? musicaFalha : musicaComemoracao);
    #endregion

    /*------------------------------------------------------------------*/
    #region Reprodutor Genérico
    private void TocarMusica(AudioClip clip, bool loop = true)
    {
        if (!clip || !musicaSource) return;

        if (musicaSource.clip == clip && musicaSource.isPlaying)
            return;

        musicaSource.clip = clip;
        musicaSource.loop = loop;
        musicaSource.time = 0f;
        musicaSource.Play();
    }


    #endregion

    /*------------------------------------------------------------------*/
    #region Volumes
    public void AtualizarVolumeMusica(float v) => SetVolume("BGMVolume", v);
    public void AtualizarVolumeSFX(float v) => SetVolume("SFXVolume", v);
    public void AtualizarVolumeMaster(float v) => SetVolume("MasterVolume", v);

    private void SetVolume(string param, float val)
    {
        float db = Mathf.Log10(Mathf.Clamp(val, 0.001f, 1f)) * 20f;
        audioMixer.SetFloat(param, db);
        PlayerPrefs.SetFloat(param, val);
    }

    private void RestaurarVolumes()
    {
        if (!PlayerPrefs.HasKey("VolumeInicializado"))
        {
            // Primeira vez abrindo o jogo — define volumes em 50%
            float volumeInicial = 0.5f;
            SetVolume("BGMVolume", volumeInicial);
            SetVolume("SFXVolume", volumeInicial);
            SetVolume("MasterVolume", volumeInicial);

            PlayerPrefs.SetFloat("BGMVolume", volumeInicial);
            PlayerPrefs.SetFloat("SFXVolume", volumeInicial);
            PlayerPrefs.SetFloat("MasterVolume", volumeInicial);

            PlayerPrefs.SetInt("VolumeInicializado", 1);
        }
        else
        {
            // Já inicializou antes — restaura os valores salvos
            SetVolume("BGMVolume", PlayerPrefs.GetFloat("BGMVolume", 1f));
            SetVolume("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 1f));
            SetVolume("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 1f));
        }
    }


    /// <summary>
    /// Procura sliders por nome se as referências estiverem nulas e sincroniza valores.
    /// </summary>
    public void SincronizarSliders()
    {
        if (!sliderMusica)
            sliderMusica = GameObject.Find("BGMVolume")?.GetComponent<Slider>();
        if (!sliderSFX)
            sliderSFX = GameObject.Find("SFXVolume")?.GetComponent<Slider>();
        if (!sliderMaster)
            sliderMaster = GameObject.Find("MasterVolume")?.GetComponent<Slider>();

        if (sliderMusica)
        {
            sliderMusica.onValueChanged.RemoveAllListeners();
            sliderMusica.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
            sliderMusica.onValueChanged.AddListener(AtualizarVolumeMusica);
        }
        if (sliderSFX)
        {
            sliderSFX.onValueChanged.RemoveAllListeners();
            sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sliderSFX.onValueChanged.AddListener(AtualizarVolumeSFX);
        }
        if (sliderMaster)
        {
            sliderMaster.onValueChanged.RemoveAllListeners();
            sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            sliderMaster.onValueChanged.AddListener(AtualizarVolumeMaster);
        }

        // Cancela o loop quando todos os sliders forem encontrados
        if (sliderMusica && sliderSFX && sliderMaster)
            CancelInvoke(nameof(SincronizarSliders));
    }


    #endregion

    /*------------------------------------------------------------------*/
    #region Mute
    public void AlternarMute()
    {
        estaMutado = !estaMutado;
        AudioListener.volume = estaMutado ? 0f : 1f;
        if (muteButton)
        {
            var img = muteButton.GetComponent<Image>();
            if (img) img.sprite = estaMutado ? somDesligadoSprite : somLigadoSprite;
        }
    }
    #endregion
}
