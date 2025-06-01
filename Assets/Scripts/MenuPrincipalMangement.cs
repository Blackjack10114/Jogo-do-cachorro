using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MenuPrincipalMangement : MonoBehaviour
{
    [SerializeField] private string nomeDoLevelDeJogo;
    [SerializeField] private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelFundoCinza;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelTutorial;

    private System.Action acaoConfirmada;

    private void Start()
    {
        if (painelConfirmacao != null)
            painelConfirmacao.SetActive(false);
    }

    public void Jogar()
    {
        // Exibe a pergunta do tutorial apenas se for a primeira vez
        if (!PlayerPrefs.HasKey("JaViuTutorialPergunta"))
        {
            painelTutorial.SetActive(true);
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
    }

    public void FecharOpcoes()
    {
        painelFundoCinza.SetActive(false);
        painelOpcoes.SetActive(false);
        painelMenuInicial.SetActive(true);
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
