using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDaFase : MonoBehaviour
{
    private SistemaPontuacao pontuacaoScript;

    void Start()
    {
        pontuacaoScript = Object.FindFirstObjectByType<SistemaPontuacao>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Calcula e salva a pontuação
            pontuacaoScript.CalcularPontuacaoFinal();
            int pontos = pontuacaoScript.GetPontuacaoFinal();
            PlayerPrefs.SetInt("PontuacaoFinal", pontos);

            // (opcional) Reseta o tempo se estiver pausado
            Time.timeScale = 1f;

            // Vai para a cena de fim de fase (certifique-se que "CenaFimDeFase" está no Build Settings)
            SceneManager.LoadScene("CenaFimDeFase");
        }
    }
}
