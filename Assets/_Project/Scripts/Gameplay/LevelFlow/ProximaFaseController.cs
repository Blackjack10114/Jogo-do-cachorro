using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ProximaFaseController : MonoBehaviour
{
    public const string CenaInicialPadrao = "Fase_TatuMafioso_01";

    [Header("Configuracoes")]
    [SerializeField] private string[] ordemFases = { CenaInicialPadrao, "Fase_Alien_02", "Fase_Dino_03" };
    [SerializeField] private Button botaoProximaFase;
    [SerializeField] private string cenaMenu = "MenuPrincipal";
    [SerializeField] private string cenaRetry;
    [SerializeField] private string cenaFimDemo = "CenaFimDeDemo";
    [SerializeField] private string ProximaFase;
    [SerializeField] private GameObject painelConfirmacao;
    private System.Action acaoConfirmada;

    [Header("Aparencia do Botao")]
    [SerializeField] private Color corCompleto = Color.gray;
    [SerializeField] private string textoCompleto = "Demo Concluida!";

    [Header("Focus Manager")]
    [SerializeField] private PanelFocusManager confirmacaoFocus;
    [SerializeField] private PanelFocusManager botoesPrincipaisFocus;

    [Header("Botoes principais da cena")]
    [SerializeField] private Button[] botoesPrincipais;
    public GameObject Sim;
    public GameObject Nao;

    private string _cenaAnterior; // Armazena a cena que chamou a vitoria

    void Start()
    {
        _cenaAnterior = ObterCenaAtualRegistrada();
        ConfigurarBotao();
    }

    private void ConfigurarBotao()
    {
        if (botaoProximaFase == null) return;

        int indiceFaseAtual = System.Array.IndexOf(ordemFases, _cenaAnterior);
        bool temProximaFase = (indiceFaseAtual >= 0) && (indiceFaseAtual < ordemFases.Length - 1);

        botaoProximaFase.interactable = temProximaFase;

        if (!temProximaFase)
        {
            botaoProximaFase.GetComponentInChildren<Text>().text = textoCompleto;
            botaoProximaFase.image.color = corCompleto;
        }
    }

    public void ProximaFaseSimples()
    {
        SceneManager.LoadScene(ProximaFase);
    }

    public void CarregarProximaFase()
    {
        int indiceFaseAtual = System.Array.IndexOf(ordemFases, _cenaAnterior);
        if (indiceFaseAtual >= 0 && indiceFaseAtual < ordemFases.Length - 1)
        {
            SceneManager.LoadScene(ordemFases[indiceFaseAtual + 1]);
        }
    }

    public void Retry()
    {
        MostrarConfirmacao(() =>
        {
            string cena = string.IsNullOrEmpty(cenaRetry) ? ObterCenaAtualRegistrada() : cenaRetry;
            SceneManager.LoadScene(cena);
        });
    }

    public void VoltarAoMenu()
    {
        MostrarConfirmacao(() =>
        {
            SceneManager.LoadScene(cenaMenu);
        });
    }

    public void FimDemo()
    {
        MostrarConfirmacao(() =>
        {
            SceneFadeUI.Instance.FadeToScene(cenaFimDemo);
            Button botao = Sim.GetComponent<Button>();
            if (botao != null)
            {
                botao.interactable = false;
            }
            Button botaonao = Nao.GetComponent<Button>();
            if (botaonao != null)
            {
                botaonao.interactable = false;
            }
        });
    }

    private void MostrarConfirmacao(System.Action acao)
    {
        botoesPrincipaisFocus.OnClose();

        painelConfirmacao.SetActive(true);
        acaoConfirmada = acao;

        foreach (var btn in botoesPrincipais)
            btn.interactable = false;

        confirmacaoFocus.OnOpen();
    }


    public void BotaoConfirmarSim()
    {
        acaoConfirmada?.Invoke();
        acaoConfirmada = null;
    }

    public void BotaoConfirmarNao()
    {
        painelConfirmacao.SetActive(false);

        confirmacaoFocus.OnClose();

        foreach (var btn in botoesPrincipais)
            btn.interactable = true;

        botoesPrincipaisFocus.OnOpen();

        acaoConfirmada = null;

    }


    public static void RegistrarCenaAtual(string cenaAtual)
    {
        if (string.IsNullOrEmpty(cenaAtual)) return;

        PlayerPrefs.SetString("CenaAnterior", cenaAtual);
        PlayerPrefs.Save();
    }

    public static string ObterCenaAtualRegistrada()
    {
        return PlayerPrefs.GetString("CenaAnterior", CenaInicialPadrao);
    }
}
