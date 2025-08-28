using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class PauseMenu : MonoBehaviour
{
    private InputController inputActions;

    [Header("Painéis")]
    [SerializeField] private GameObject painelPause;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelFundoCinza;
    [SerializeField] private GameObject painelComoJogar;

    [Header("Focus Managers")]
    [SerializeField] private PanelFocusManager pauseFocus;
    [SerializeField] private PanelFocusManager confirmacaoFocus;
    [SerializeField] private PanelFocusManager comojogarFocus;

    public static bool EstaPausado() => JogoPausado;
    PlayerMov PlayerMov;
    Jump Jump;
    public static bool JogoPausado { get; private set; }
    private System.Action acaoConfirmada;
   
    void Awake()
    {
        inputActions = new InputController();
        PlayerMov = FindFirstObjectByType<PlayerMov>();
        Jump = FindFirstObjectByType<Jump>();

        //  abrir e fechar pause com input
        inputActions.Player.Pause.performed += ctx =>
        {
            var comeco = FindFirstObjectByType<Comeco_Fase>();
            if (comeco == null) // Cena sem Comeco_Fase (tipo tutorial)
            {
                // Pode pausar sempre
                if (!painelPause.activeSelf)
                    AbrirPause();
                else
                    FecharPause();
            }
            else if (Comeco_Fase.FaseComecou) // Cena com Comeco_Fase, mas só depois que começou
            {
                if (!painelPause.activeSelf)
                    AbrirPause();
                else
                    FecharPause();
            }


        };

        // fechar a confirmação
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
        JogoPausado = true;
        PlayerMov.ResetarInput();
        PlayerMov.PararSomCorrida();
        Jump.PararSomPulo();
        Time.timeScale = 0f;

        painelFundoCinza.SetActive(true);
        painelPause.SetActive(true);
        painelConfirmacao.SetActive(false);

        inputActions.Player.Disable();
        inputActions.UI.Enable();

        pauseFocus.OnOpen();

        Debug.Log("Pause aberto");
    }

    public void FecharPause()
    {
        StartCoroutine(FecharPauseComDelay());
    }

    public void BotaoComoJogar()
    {
        pauseFocus.OnClose();

        painelPause.SetActive(false);
        painelComoJogar.SetActive(true);

        comojogarFocus.OnOpen();
    }

    public void BotaoVoltarPause()
    {
        comojogarFocus.OnClose();

        painelComoJogar.SetActive(false);
        painelPause.SetActive(true);

        pauseFocus.OnOpen();
    }
    public void BotaoContinuar()
    {
        FecharPause();
    }

    public void BotaoMenuPrincipal()
    {
        MostrarConfirmacao(() =>
        {
            JogoPausado = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuPrincipal");
        });
    }

    public void BotaoReiniciar()
    {
        MostrarConfirmacao(() =>
        {
            JogoPausado = false;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    private void MostrarConfirmacao(System.Action acao)
    {
        pauseFocus.OnClose();

        painelPause.SetActive(false);
        painelConfirmacao.SetActive(true);

        acaoConfirmada = acao;

        confirmacaoFocus.OnOpen();
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
        painelPause.SetActive(true);

        pauseFocus.OnOpen();

        acaoConfirmada = null;
    }

    void OnDestroy()
    {
        if (JogoPausado)
        {
            Time.timeScale = 1f;
            JogoPausado = false;
        }
    }
    private IEnumerator FecharPauseComDelay()
    {
        // garante que o jogo não mexe ainda
        inputActions.Player.Disable();

        // espera alguns ms no tempo real (ignora o timescale)
        yield return new WaitForSecondsRealtime(0.1f);

        // reseta o estado do botão de pulo
        var jumpAction = inputActions.Player.Jump;
        jumpAction.Reset();  // limpa valores acumulados

        // só depois libera o jogo
        JogoPausado = false;
        Time.timeScale = 1f;

        painelFundoCinza.SetActive(false);
        painelPause.SetActive(false);
        painelConfirmacao.SetActive(false);

        inputActions.UI.Disable();
        inputActions.Player.Enable();

        pauseFocus.OnClose();

        Debug.Log("Pause fechado com delay");
    }
}
