using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Controlador_Som : MonoBehaviour
{
    public static Controlador_Som instancia;

    [Header("Referências")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private AudioSource musicaSource;

    [Header("Configurações do Mute")]
    [SerializeField] private Button muteButton;
    [SerializeField] private Sprite somLigadoSprite;
    [SerializeField] private Sprite somDesligadoSprite;

    private bool estaMutado = false;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject); 
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject); 

        if (musicaSource != null)
        {
            float tempoSalvo = PlayerPrefs.GetFloat("MusicaTempo", 0f);
            musicaSource.time = tempoSalvo;
            musicaSource.Play();
        }
    }

    void Start()
    {
        float volMusica = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float volSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float volMaster = PlayerPrefs.GetFloat("MasterVolume", 1f);

        if (sliderMusica != null) sliderMusica.value = volMusica;
        if (sliderSFX != null) sliderSFX.value = volSFX;
        if (sliderMaster != null) sliderMaster.value = volMaster;

        SetVolume("MusicVolume", volMusica);
        SetVolume("SFXVolume", volSFX);
        SetVolume("MasterVolume", volMaster);
    }

    void Update()
    {
        if (musicaSource != null && musicaSource.isPlaying)
        {
            PlayerPrefs.SetFloat("MusicaTempo", musicaSource.time);
        }
    }

    public void AtualizarVolumeMusica(float valor)
    {
        SetVolume("MusicVolume", valor);
    }

    public void AtualizarVolumeSFX(float valor)
    {
        SetVolume("SFXVolume", valor);
    }

    public void AtualizarVolumeMaster(float valor)
    {
        SetVolume("MasterVolume", valor);
    }

    private void SetVolume(string parametro, float valor)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(valor, 0.001f, 1f)) * 20f;
        audioMixer.SetFloat(parametro, volumeDb);
        PlayerPrefs.SetFloat(parametro, valor);
        PlayerPrefs.Save();
    }

    public void AlternarMute()
    {
        estaMutado = !estaMutado;
        AudioListener.volume = estaMutado ? 0f : 1f;

        if (muteButton != null)
        {
            Image img = muteButton.GetComponent<Image>();
            if (img != null)
                img.sprite = estaMutado ? somDesligadoSprite : somLigadoSprite;
        }
    }
}
