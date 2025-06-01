using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFim : MonoBehaviour
{
    [SerializeField] private GameObject painelFimTutorial;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelFundoCinza;

    private System.Action acaoConfirmada;
    public void ProximaFaseTutorial()
    {
        Time.timeScale = 1f;
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
        painelFimTutorial.SetActive(false);
        acaoConfirmada = acao;
    }

    public void BotaoConfirmarSim()
    {
        Time.timeScale = 1f;
        acaoConfirmada?.Invoke();
        acaoConfirmada = null;
    }

    public void BotaoConfirmarNao()
    {
        painelFimTutorial.SetActive(true);
        painelConfirmacao.SetActive(false);
    }
}
