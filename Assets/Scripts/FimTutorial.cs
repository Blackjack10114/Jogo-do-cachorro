using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDeFaseTutorial : MonoBehaviour
{
    public GameObject painelConfirmacao;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f; // pausa o jogo
            painelConfirmacao.SetActive(true);
        }
    }

    public void BotaoSim()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void BotaoNao()
    {
        Time.timeScale = 1f;
        painelConfirmacao.SetActive(false);
    }
}
