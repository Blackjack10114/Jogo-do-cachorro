using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Controlador_Som : MonoBehaviour
{
    public static Controlador_Som instancia;

    [Header("Mixer e Áudio")]
    public AudioMixer AudioMixer;

    [SerializeField] private AudioSource musicaSource;

    [Header("Músicas")]
    [SerializeField] private AudioClip musicaMenu;
    [SerializeField] private AudioClip musicaTutorial;
    [SerializeField] private AudioClip musicaTatu;
    [SerializeField] private AudioClip musicaAlien;
    [SerializeField] private AudioClip musicaDino;
    [SerializeField] private AudioClip musicaFalha;
    [SerializeField] private AudioClip musicaComemoracao;

    private void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject); // já existe outro, destrói este
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        AplicarVolumesIniciais();

        if (musicaSource == null)
        {
            musicaSource = gameObject.AddComponent<AudioSource>();
            musicaSource.playOnAwake = false;
            musicaSource.loop = true;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene cena, LoadSceneMode modo)
    {
        // Se for cena de fim de fase, verifica nota e decide música
        if (cena.name.StartsWith("CenaFimDeFase"))
        {
            string nota = PlayerPrefs.GetString("NotaFinal", "S"); // Nota salva no fim da fase

            if (nota == "F" || nota == "C+" || nota == "C" || nota == "C-")
            {
                TocarMusica(musicaFalha, false);
                return;
            }
            else
            {
                TocarMusica(musicaComemoracao, false);
                return;
            }
        }

        TrocarMusicaPorCena(cena.name); // cena normal
    }


    public void TrocarMusicaPorCena(string nomeCena)
    {
        AudioClip novaMusica = musicaMenu;
        bool loop = true;

        switch (nomeCena)
        {
            case "Tutorial": novaMusica = musicaTutorial; break;
            case "Fase_TatuMafioso_01": novaMusica = musicaTatu; break;
            case "Fase_Alien_02": novaMusica = musicaAlien; break;
            case "Fase_Dino_03": novaMusica = musicaDino; break;

            case "CenaFalhaTatu":
            case "CenaFalhaAlien":
            case "CenaFalhaDino":
                novaMusica = musicaFalha;
                loop = false;
                break;

            case "MenuPrincipal":
            case "CenaSelecaoFase":
            case "Creditos":
            default:
                novaMusica = musicaMenu;
                break;
        }

        TocarMusica(novaMusica, loop);
    }


    public void TocarMusica(AudioClip clip, bool loop = true)
    {
        if (musicaSource.clip == clip && musicaSource.isPlaying)
            return;

        musicaSource.loop = loop;
        musicaSource.clip = clip;
        musicaSource.time = 0f;
        musicaSource.Play();
    }
    private void AplicarVolumesIniciais()
    {
        AplicarVolume("MasterVolume");
        AplicarVolume("BGMVolume");
        AplicarVolume("SFXVolume");
    }
    private void AplicarVolume(string parametro)
    {
        float valor = PlayerPrefs.GetFloat(parametro, 1f); // 1f = volume padrão
        float db = Mathf.Log10(Mathf.Clamp(valor, 0.001f, 1f)) * 20f;
        AudioMixer.SetFloat(parametro, db);
    }

}
