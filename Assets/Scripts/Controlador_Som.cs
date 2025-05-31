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
    [Header("Mixer e UI (opcional)")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderMusica;  // Se vazio, será buscado por nome "SliderMusica"
    [SerializeField] private Slider sliderSFX;     // Se vazio, busca "SliderSFX"
    [SerializeField] private Slider sliderMaster;  // Se vazio, busca "SliderMaster"

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
        SincronizarSliders();
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
        switch (nome)
        {
            case "Tutorial": alvo = musicaTutorial; break;
            case "Fase_TatuMafioso_01": alvo = musicaTatu; break;
            case "Fase_Alien_02": alvo = musicaAlien; break;
            case "Fase_Dino_03": alvo = musicaDino; break;
            case "CenaFalhaTatu":
            case "CenaFalhaAlien":
            case "CenaFalhaDino": alvo = musicaFalha; break;
            case "CenaFimDeFaseTatu":
            case "CenaFimDeFaseAlien":
            case "CenaFimDeFaseDino": alvo = musicaComemoracao; break;
        }
        TocarMusica(alvo);
    }
    #endregion

    /*------------------------------------------------------------------*/
    #region API – Música fim de fase
    public void DefinirMusicaFimDeFase(bool falhou)
        => TocarMusica(falhou ? musicaFalha : musicaComemoracao);
    #endregion

    /*------------------------------------------------------------------*/
    #region Reprodutor Genérico
    private void TocarMusica(AudioClip clip)
    {
        if (!clip || !musicaSource) return;
        if (musicaSource.clip == clip && musicaSource.isPlaying) return;
        musicaSource.clip = clip;
        musicaSource.time = (clip == musicaMenu) ? PlayerPrefs.GetFloat("MusicaTempo", 0f) : 0f;
        musicaSource.Play();
    }
    #endregion

    /*------------------------------------------------------------------*/
    #region Volumes
    public void AtualizarVolumeMusica(float v) => SetVolume("MusicVolume", v);
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
        SetVolume("MusicVolume", PlayerPrefs.GetFloat("MusicVolume", 1f));
        SetVolume("SFXVolume", PlayerPrefs.GetFloat("SFXVolume", 1f));
        SetVolume("MasterVolume", PlayerPrefs.GetFloat("MasterVolume", 1f));
    }

    /// <summary>
    /// Procura sliders por nome se as referências estiverem nulas e sincroniza valores.
    /// </summary>
    private void SincronizarSliders()
    {
        if (!sliderMusica) sliderMusica = GameObject.Find("BGMVolume")?.GetComponent<Slider>();
        if (!sliderSFX) sliderSFX = GameObject.Find("SFXVolume")?.GetComponent<Slider>();
        if (!sliderMaster) sliderMaster = GameObject.Find("MasterVolume")?.GetComponent<Slider>();

        if (sliderMusica) sliderMusica.value = PlayerPrefs.GetFloat("BGMVolume", 1f);
        if (sliderSFX) sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        if (sliderMaster) sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 1f);

        // Liga eventos se ainda não estiverem conectados
        if (sliderMusica && sliderMusica.onValueChanged.GetPersistentEventCount() == 0)
            sliderMusica.onValueChanged.AddListener(AtualizarVolumeMusica);
        if (sliderSFX && sliderSFX.onValueChanged.GetPersistentEventCount() == 0)
            sliderSFX.onValueChanged.AddListener(AtualizarVolumeSFX);
        if (sliderMaster && sliderMaster.onValueChanged.GetPersistentEventCount() == 0)
            sliderMaster.onValueChanged.AddListener(AtualizarVolumeMaster);
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
