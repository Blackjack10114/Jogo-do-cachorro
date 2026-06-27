using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MostrarTempo : MonoBehaviour
{
    private GameObject contador;
    private FimDeFaseUI tempofinal;
    private bool tempomostrado;
    public LocalizedString localStringNota;
    public Text texttempo;

    private void Start()
    {
        contador = GameObject.FindGameObjectWithTag("Pontos");
        tempofinal = contador.GetComponent<FimDeFaseUI>();
    }

    private void Update()
    {
        if (tempomostrado == false)
        {
            MostrarTempos();
        }
        else return;
    }

    private void OnEnable()
    {
        localStringNota.StringChanged += UpdateText;
    }

    private void OnDisable()
    {
        localStringNota.StringChanged -= UpdateText;
    }

    private void UpdateText(string value)
    {
        texttempo.text = value;
    }

    public void MostrarTempos()
    {
        float tempoArredondado = Mathf.Round(tempofinal.tempo * 10f) / 10f;
        localStringNota.Arguments = new object[] { tempoArredondado };
        localStringNota.RefreshString();
        Debug.LogWarning("[MostrarTempo] Tempo arredondado usado na LocalizedString: " + tempoArredondado);
    }

}
