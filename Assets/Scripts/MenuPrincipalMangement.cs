using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuPrincipalMangement : MonoBehaviour
{
    private InputController inputActions;

    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelFundoCinza;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelTutorial;
    [SerializeField] private GameObject painelComoJogar;

   [Header("Focus Managers")]
    [SerializeField] private PanelFocusManager menuInicialFocus;
    [SerializeField] private PanelFocusManager opcoesFocus;
    [SerializeField] private PanelFocusManager confirmacaoFocus;
    [SerializeField] private PanelFocusManager tutorialFocus;
    [SerializeField] private PanelFocusManager comojogarFocus;

    private System.Action acaoConfirmada;

    void Awake()
    {
        inputActions = new InputController();

        // ESC para fechar opções e voltar ao menu inicial
        inputActions.UI.Cancel.performed += ctx =>
        {
            if (painelOpcoes.activeSelf)
            {
                FecharOpcoes();
            }
        };
    }

    void OnEnable()
    {
        inputActions.Enable();
        inputActions.Player.Disable(); // desabilita controles do jogador
        inputActions.UI.Enable();      // habilita controles de UI
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        if (painelConfirmacao != null)
            painelConfirmacao.SetActive(false);

        menuInicialFocus.OnOpen();
    }

    public void Jogar()
    {
        if (!PlayerPrefs.HasKey("JaViuTutorialPergunta"))
        {
            menuInicialFocus.OnClose();

            painelTutorial.SetActive(true);
            tutorialFocus.OnOpen();
        }
        else
        {
            SceneManager.LoadScene("CenaSelecaoFase");
        }
    }

    public void AbrirOpcoes()
    {
        menuInicialFocus.OnClose();

        painelMenuInicial.SetActive(false);
        painelFundoCinza.SetActive(true);
        painelOpcoes.SetActive(true);

        opcoesFocus.OnOpen();
    }

    public void FecharOpcoes()
    {
        opcoesFocus.OnClose();

        painelOpcoes.SetActive(false);
        painelFundoCinza.SetActive(false);
        painelMenuInicial.SetActive(true);

        menuInicialFocus.OnOpen();
    }

    public void SairJogo()
    {
        MostrarConfirmacao(() =>
        {
            Debug.Log("Sair do Jogo");
            Application.Quit();
        });
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void MostrarConfirmacao(System.Action acao)
    {
        menuInicialFocus.OnClose();

        painelMenuInicial.SetActive(false);
        painelConfirmacao.SetActive(true);

        confirmacaoFocus.OnOpen();

        acaoConfirmada = acao;
    }

    public void BotaoConfirmarSim()
    {
        acaoConfirmada?.Invoke();
        acaoConfirmada = null;
    }

    public void BotaoConfirmarNao()
    {
        confirmacaoFocus.OnClose();

        painelConfirmacao.SetActive(false);
        painelMenuInicial.SetActive(true);

        menuInicialFocus.OnOpen();
    }

    public void BotaoCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void BotaoTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void TutorialSim()
    {
        PlayerPrefs.SetInt("JaViuTutorialPergunta", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Tutorial");
    }

    public void TutorialNao()
    {
        PlayerPrefs.SetInt("JaViuTutorialPergunta", 1);
        PlayerPrefs.Save();

        tutorialFocus.OnClose();

        painelTutorial.SetActive(false);
        SceneManager.LoadScene("CenaSelecaoFase");
    }

    public void BotaoComoJogar()
    {
        opcoesFocus.OnClose();

        painelOpcoes.SetActive(false);
        painelComoJogar.SetActive(true);

        comojogarFocus.OnOpen();
    }
    public void BotaoVoltarOpcoes()
    {
        comojogarFocus.OnClose();

        painelComoJogar.SetActive(false);
        painelOpcoes.SetActive(true);

        opcoesFocus.OnOpen();
    }
}
