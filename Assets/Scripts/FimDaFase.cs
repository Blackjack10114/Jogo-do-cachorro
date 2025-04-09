using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDaFase : MonoBehaviour
{
    private SistemaPontuacao pontuacaoScript;
    private Dano danoScript;

    public GameObject avisoFaltaCaixaUI;

    void Start()
    {
        pontuacaoScript = Object.FindFirstObjectByType<SistemaPontuacao>();
        danoScript = Object.FindFirstObjectByType<Dano>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Verifica se está com o sprite sem a caixa (indicando que a caixa foi solta)
            var spriteAtual = other.GetComponent<SpriteRenderer>().sprite;

            if (spriteAtual == danoScript.Sprite_Dog_Sem_Caixa)
            {
                if (avisoFaltaCaixaUI != null)
                    avisoFaltaCaixaUI.SetActive(true);

                Debug.Log("❌ A entrega não foi feita! Volte e recupere a caixa.");
                StartCoroutine(ResetarEntrada());
                return;
            }

            // Calcula a pontuação final
            pontuacaoScript.CalcularPontuacaoFinal();

            // Salva os dados para serem usados na próxima cena
            int pontos = pontuacaoScript.GetPontuacaoFinalNumerica();
            string nota = pontuacaoScript.GetClassificacaoLetra();

            PlayerPrefs.SetInt("PontuacaoFinal", pontos);
            PlayerPrefs.SetString("NotaFinal", nota);

            Time.timeScale = 1f;

            SceneManager.LoadScene("CenaFimDeFase");
        }
    }

    private IEnumerator ResetarEntrada()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(2f);

        if (avisoFaltaCaixaUI != null)
            avisoFaltaCaixaUI.SetActive(false);

        GetComponent<Collider2D>().enabled = true;
    }
}
