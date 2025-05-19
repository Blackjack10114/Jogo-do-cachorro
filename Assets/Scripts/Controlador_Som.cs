using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Controlador_Som : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sliderMusica;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderMaster;

    [Header("Configurações do Mute")]
    [SerializeField] private Button muteButton;
    [SerializeField] private Sprite somLigadoSprite;
    [SerializeField] private Sprite somDesligadoSprite;

    void Start()
    {
        float volMusica = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float volSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float volMaster = PlayerPrefs.GetFloat("MasterVolume", 1f);

        sliderMusica.value = volMusica;
        sliderSFX.value = volSFX;
        sliderMaster.value = volMaster;

        SetVolume("MusicVolume", volMusica);
        SetVolume("SFXVolume", volSFX);
        SetVolume("MasterVolume", volMaster);
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
}
