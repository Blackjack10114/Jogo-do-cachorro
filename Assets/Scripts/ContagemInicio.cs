using System.Collections;
using UnityEngine;
using TMPro;

public class ContagemInicio : MonoBehaviour
{
    public GameObject painelContagem; // Painel com o texto
    public TextMeshProUGUI textoContagem; // Texto exibido na contagem

    void Start()
    {
        StartCoroutine(Contagem());
    }

    IEnumerator Contagem()
    {
        Time.timeScale = 0f;
        painelContagem.SetActive(true);

        string[] numeros = { "3", "2", "1", "ENTREGUE!" };

        foreach (string num in numeros)
        {
            textoContagem.text = num;
            yield return new WaitForSecondsRealtime(1f);
        }

        painelContagem.SetActive(false);
        Time.timeScale = 1f;
    }
}
