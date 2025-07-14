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
    [SerializeField] private GameObject botaoInicial; 
    [SerializeField] private GameObject botaoInicialScroll;
    [SerializeField] private GameObject botaoInicialTutorial;
    [SerializeField] private GameObject botaoInicialConfirmacao;
  

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
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoInicial);

        if (painelConfirmacao != null)
            painelConfirmacao.SetActive(false);
    }

    public void Jogar()
    {
        if (!PlayerPrefs.HasKey("JaViuTutorialPergunta"))
        {
            painelTutorial.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(botaoInicialTutorial);
        }
        else
        {
            SceneManager.LoadScene("CenaSelecaoFase");
        }
    }

    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelFundoCinza.SetActive(true);
        painelOpcoes.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoInicialScroll);
    }

    public void FecharOpcoes()
    {
        painelOpcoes.SetActive(false);
        painelFundoCinza.SetActive(false);
        painelMenuInicial.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoInicial);
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
        painelMenuInicial.SetActive(false);
        painelConfirmacao.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoInicialConfirmacao);
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
        painelMenuInicial.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoInicial);
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
        painelTutorial.SetActive(false);
        SceneManager.LoadScene("CenaSelecaoFase");
    }
}
