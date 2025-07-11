using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private InputController inputActions;

    [Header("Painéis")]
    [SerializeField] private GameObject painelPause;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelFundoCinza;
    [SerializeField] private GameObject botaoInicial;

    private System.Action acaoConfirmada;

    void Awake()
    {
        inputActions = new InputController();

        // quando o jogador aperta Pause no mapa UI
        inputActions.UI.Pause.performed += ctx =>
        {
            if (!painelPause.activeSelf)
                AbrirPause();
            else
                FecharPause();
        };

        // opcionalmente já ouça o Confirm / Cancel também aqui
        inputActions.UI.Confirm.performed += ctx =>
        {
            if (painelConfirmacao.activeSelf)
                BotaoConfirmarSim();
        };

        inputActions.UI.Cancel.performed += ctx =>
        {
            if (painelConfirmacao.activeSelf)
                BotaoConfirmarNao();
        };
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    public void AbrirPause()
    {
        Time.timeScale = 0f;
        painelFundoCinza.SetActive(true);
        painelPause.SetActive(true);
        painelConfirmacao.SetActive(false);

        // desabilita o mapa do jogador e habilita UI
        inputActions.Player.Disable();
        inputActions.UI.Enable();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoInicial);
    }

    public void FecharPause()
    {
        Time.timeScale = 1f;
        painelFundoCinza.SetActive(false);
        painelPause.SetActive(false);
        painelConfirmacao.SetActive(false);

        // reabilita o mapa do jogador e desabilita UI
        inputActions.UI.Disable();
        inputActions.Player.Enable();
    }

    public void BotaoContinuar()
    {
        FecharPause();
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
        painelPause.SetActive(false);
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
        painelPause.SetActive(true);
    }
}
