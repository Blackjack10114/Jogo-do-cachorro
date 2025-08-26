using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class MudaNota : MonoBehaviour
{
    private GameObject contador;
    private FimDeFaseUI notafinal;
    private bool notamostrada;

    public LocalizedString localStringNota;
    public Text textnota;

    private void Start()
    {
        contador = GameObject.FindGameObjectWithTag("Pontos");
        notafinal = contador.GetComponent<FimDeFaseUI>();
        Debug.LogWarning("[Start] Nota encontrada: " + notafinal.nota);
    }
    private void Update()
    {
        if (notamostrada == false)
        {
            MostrarNota();
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
        textnota.text = value;
    }

    public void MostrarNota()
    {
        localStringNota.Arguments = new object[] { notafinal.nota };
        localStringNota.RefreshString();
        Debug.LogWarning("[MostrarNota] Nota usada na LocalizedString: " + notafinal.nota);
    }
}
