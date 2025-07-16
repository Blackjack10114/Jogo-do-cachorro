using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFim : MonoBehaviour
{
    public GameObject painelFimTutorial;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelFundoCinza;

    [Header("Focus Managers")]
    [SerializeField] private PanelFocusManager fimTutorialFocus;
    [SerializeField] private PanelFocusManager confirmacaoFocus;

    private System.Action acaoConfirmada;

    /// <summary>
    /// Mostra o painel de fim do tutorial com foco.
    /// Chame este método ao invés de só SetActive(true) no painel.
    /// </summary>
    public void MostrarFimTutorial()
    {
        painelFundoCinza.SetActive(true);
        painelFimTutorial.SetActive(true);
        painelConfirmacao.SetActive(false);

        fimTutorialFocus.OnOpen(); // força o foco no botão padrão
    }

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

        fimTutorialFocus.OnClose();
        confirmacaoFocus.OnOpen();

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

        confirmacaoFocus.OnClose();
        fimTutorialFocus.OnOpen();

        acaoConfirmada = null;
    }
}
