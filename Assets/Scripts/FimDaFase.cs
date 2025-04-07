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
            // Calcula a pontuação final
            pontuacaoScript.CalcularPontuacaoFinal();

            // Salva os dados para serem usados na próxima cena
            int pontos = pontuacaoScript.GetPontuacaoFinalNumerica();
            string nota = pontuacaoScript.GetClassificacaoLetra();


            PlayerPrefs.SetInt("PontuacaoFinal", pontos);
            PlayerPrefs.SetString("NotaFinal", nota);

            Time.timeScale = 1f;

            // Carrega a cena de fim de fase
            SceneManager.LoadScene("CenaFimDeFase");
        }
    }
}
