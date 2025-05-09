using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Controlador_Som : MonoBehaviour
{
    private bool estadoSom = true;

    [SerializeField] private AudioSource fundoMusical;
    [SerializeField] private Sprite somLigadoSprite;
    [SerializeField] private Sprite somDesligadoSprite;
    [SerializeField] private Image muteImage;
    [SerializeField] private Slider volumeSlider; // ← Atribua o Slider no Inspector

    void Start()
    {
        float volumeSalvo = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        fundoMusical.volume = volumeSalvo;
        if (volumeSlider != null)
        {
            volumeSlider.value = volumeSalvo;
        }
    }

    public void LigarDesligarSom()
    {
        estadoSom = !estadoSom;
        fundoMusical.enabled = estadoSom;

        muteImage.sprite = estadoSom ? somLigadoSprite : somDesligadoSprite;
    }

    public void VolumeMusical(float value)
    {
        fundoMusical.volume = value;
        PlayerPrefs.SetFloat("VolumeMusica", value);
        PlayerPrefs.Save();
    }
}
