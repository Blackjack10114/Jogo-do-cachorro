using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProximaFaseController: MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private string[] ordemFases = { "Fase_Playtest", "Fase_Alien_02" };
    [SerializeField] private Button botaoProximaFase;
    [SerializeField] private string cenaMenu = "MenuPrincipal";
    [SerializeField] private string cenaRetry;
    [SerializeField] private string ProximaFase;
    [SerializeField] private GameObject painelConfirmacao;
    private System.Action acaoConfirmada;

    [Header("Aparência do Botão")]
    [SerializeField] private Color corCompleto = Color.gray;
    [SerializeField] private string textoCompleto = "Demo Concluída!";

    private string _cenaAnterior; // Armazena a cena que chamou a vitória

    void Start()
    {
        // Detecta automaticamente a cena anterior
        _cenaAnterior = PlayerPrefs.GetString("CenaAnterior", "Fase_Playtest");

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
        if (indiceFaseAtual < ordemFases.Length - 1)
        {
            SceneManager.LoadScene(ordemFases[indiceFaseAtual + 1]);
        }
    }
    public void Retry()
    {
        MostrarConfirmacao(() =>
        {
            SceneManager.LoadScene(cenaRetry);
        });
    }
    public void VoltarAoMenu()
    {
        MostrarConfirmacao(() =>
        {
            SceneManager.LoadScene(cenaMenu);
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

    // Chamar ANTES de carregar a cena de vitória
    public static void RegistrarCenaAtual(string cenaAtual)
    {
        PlayerPrefs.SetString("CenaAnterior", cenaAtual);
    }
}