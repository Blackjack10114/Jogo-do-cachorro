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
        sliderMusica.value = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        sliderSFX.value = PlayerPrefs.GetFloat("VolumeSFX", 1f);
        sliderMaster.value = PlayerPrefs.GetFloat("VolumeMaster", 1f);

        AtualizarVolumes();
    }

    public void AtualizarVolumes()
    {
        SetVolume("VolumeMusica", sliderMusica.value);
        SetVolume("VolumeSFX", sliderSFX.value);
        SetVolume("VolumeMaster", sliderMaster.value);
    }

    private void SetVolume(string parametro, float valor)
    {
        float volumeDb = Mathf.Log10(Mathf.Clamp(valor, 0.001f, 1f)) * 20f;
        audioMixer.SetFloat(parametro, volumeDb);
        PlayerPrefs.SetFloat(parametro, valor);
        PlayerPrefs.Save();
    }
}
