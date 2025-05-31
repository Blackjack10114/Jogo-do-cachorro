using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFim : MonoBehaviour
{
    [SerializeField] private GameObject painelConfirmacao;

    private System.Action acaoConfirmada;
    public void ProximaFaseTutorial()
    {
        SceneManager.LoadScene("Fase_TatuMafioso_01");
    }

    public void BotaoMenuPrincipal()
    {
        MostrarConfirmacao(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuPrincipal");
        });
    }

    public void BotaoReiniciar()
    {
        MostrarConfirmacao(() =>
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
    private void MostrarConfirmacao(System.Action acao)
    {
        painelConfirmacao.SetActive(true);
        acaoConfirmada = acao;
    }

    public void BotaoConfirmarSim()
    {
        acaoConfirmada?.Invoke();
        acaoConfirmada = null;
    }

    public void BotaoConfirmarNao()
    {
        painelConfirmacao.SetActive(false);
    }
}
