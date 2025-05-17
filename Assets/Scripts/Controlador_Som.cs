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

    void Start()
    {
        sliderMusica.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", 1f);

        AtualizarVolumes();
    }

    public void AtualizarVolumes()
    {
        SetVolume("MusicVolume", sliderMusica.value);
        SetVolume("SFXVolume", sliderSFX.value);
        SetVolume("MasterVolume", sliderMaster.value);
    }

    private void SetVolume(string parametro, float valor)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(valor, 0.001f, 1f)) * 20f;
        audioMixer.SetFloat(parametro, volumeDb);
        PlayerPrefs.SetFloat(parametro, valor);
        PlayerPrefs.Save();
    }
}
