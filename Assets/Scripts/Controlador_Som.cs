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
    [SerializeField] private AudioClip musicaFase;
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
        TrocarMusicaPorCena(cena.name);
    }

    public void TrocarMusicaPorCena(string nomeCena)
    {
        AudioClip novaMusica = musicaMenu;
        bool loop = true;

        switch (nomeCena)
        {
            case "Fase_TatuMafioso_01":
            case "Fase_Alien_02":
            case "Fase_Dino_03":
                novaMusica = musicaFase;
                break;

            case "CenaFimDeFaseTatu":
            case "CenaFimDeFaseAlien":
            case "CenaFimDeFaseDino":
                novaMusica = musicaComemoracao;
                loop = false;
                break;

            case "CenaFalhaTatu":
            case "CenaFalhaAlien":
            case "CenaFalhaDino":
                novaMusica = musicaFalha;
                loop = false;
                break;

            case "MenuPrincipal":
            case "CenaSelecaoFase":
            case "Creditos":
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

}
