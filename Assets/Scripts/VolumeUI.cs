using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    [Header("Parâmetro do Audio Mixer")]
    public string mixerParameter = "MasterVolume";

    [Header("Slider (arraste o próprio)")]
    public Slider slider;

    private void Awake()
    {
        if (slider == null)
            slider = GetComponent<Slider>();

        float valorInicial = PlayerPrefs.GetFloat(mixerParameter, 1f);
        slider.value = valorInicial;
        AplicarVolume(valorInicial);

        slider.onValueChanged.AddListener(AplicarVolume);
    }

    private void AplicarVolume(float valor)
    {
        float db = Mathf.Log10(Mathf.Clamp(valor, 0.001f, 1f)) * 20f;
        if (Controlador_Som.instancia != null)
        {
            Controlador_Som.instancia.AudioMixer.SetFloat(mixerParameter, db);
        }


        PlayerPrefs.SetFloat(mixerParameter, valor);
        PlayerPrefs.Save();
    }
}
