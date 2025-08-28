using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialFim : MonoBehaviour
{
    public GameObject painelFimTutorial;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelFundoCinza;

    [Header("Focus Managers")]
    [SerializeField] private PanelFocusManager fimTutorialFocus;
    [SerializeField] private PanelFocusManager confirmacaoFocus;

    private System.Action acaoConfirmada;
    private InputController inputActions;
    private PlayerMov playerMov;
    private Jump jump;

    public static bool BloquearInput { get; private set; }


    void Awake()
    {
        inputActions = new InputController();
        playerMov = FindFirstObjectByType<PlayerMov>();
        jump = FindFirstObjectByType<Jump>();
    }

    /// <summary>
    /// Mostra o painel de fim do tutorial com foco e bloqueia o input do jogador.
    /// </summary>
    public void MostrarFimTutorial()
    {
        // trava o tempo
        Time.timeScale = 0f;
        if (playerMov != null) playerMov.enabled = false;
        if (jump != null) jump.enabled = false;
        TutorialFim.BloquearInput = true;

        // Reseta e corta sons
        if (playerMov != null)
        {
            playerMov.ResetarInput();
            playerMov.PararSomCorrida();
        }
        if (jump != null)
            jump.PararSomPulo();

        // Painéis
        painelFundoCinza.SetActive(true);
        painelFimTutorial.SetActive(true);
        painelConfirmacao.SetActive(false);

        // troca inputs igual pause
        inputActions.Player.Disable();
        inputActions.UI.Enable();

        fimTutorialFocus.OnOpen();

        Debug.Log("FimTutorial aberto");
    }

    public void ProximaFaseTutorial()
    {
        FecharFimTutorial();
        SceneManager.LoadScene("Fase_TatuMafioso_01");
    }

    private void FecharFimTutorial()
    {
        TutorialFim.BloquearInput = false;
        if (playerMov != null) playerMov.enabled = true;
        if (jump != null) jump.enabled = true;
        Time.timeScale = 1f;

        painelFundoCinza.SetActive(false);
        painelFimTutorial.SetActive(false);
        painelConfirmacao.SetActive(false);

        inputActions.UI.Disable();
        inputActions.Player.Enable();

        fimTutorialFocus.OnClose();

        Debug.Log("FimTutorial fechado");
    }


    public void BotaoMenuPrincipal()
    {
        MostrarConfirmacao(() =>
        {
            FecharFimTutorial();
            SceneManager.LoadScene("MenuPrincipal");
        });
    }

    public void BotaoReiniciar()
    {
        MostrarConfirmacao(() =>
        {
            FecharFimTutorial();

            //fade para reiniciar
            string cenaAtual = SceneManager.GetActiveScene().name;
            SceneFadeUI.Instance.FadeToScene(cenaAtual);
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
