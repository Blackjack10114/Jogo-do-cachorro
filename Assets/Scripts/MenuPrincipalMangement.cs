using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MenuPrincipalMangement : MonoBehaviour
{
    [SerializeField]private string nomeDoLevelDeJogo;
    [SerializeField]private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private string CenaRetry;
    [SerializeField] private GameObject painelConfirmacao;
    [SerializeField] private GameObject painelTutorial;


    private System.Action acaoConfirmada;
    public void Jogar()
    {
        AbrirPainelTutorial();
        
    }
    public void Retry()
    {
        SceneManager.LoadScene(CenaRetry);
    }
    public void AbrirOpcoes()
    {
        painelMenuInicial.SetActive(false);
        painelOpcoes.SetActive(true);
    }
    public void FecharOpcoes()
    {
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
    
    public void ProximaFase()
    {
        SceneManager.LoadScene("Fase_Alien_02");
    }
    private void MostrarConfirmacao(System.Action acao)
    {
        painelConfirmacao.SetActive(true);
        acaoConfirmada = acao;
    }
    public void BotaoConfirmarSim()
    {
        acaoConfirmada?.Invoke();
    }

    public void BotaoConfirmarNao()
    {
        painelConfirmacao.SetActive(false);
    }

    public void BotaoCreditos()
    {
        SceneManager.LoadScene("Creditos");
    }

    public void BotaoTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void AbrirPainelTutorial()
    {
        //painelMenuInicial.SetActive(false);
        painelTutorial.SetActive(true);
    }
    public void TutorialSim()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void TutorialNao()
    {
        painelTutorial.SetActive(false);
        SceneManager.LoadScene("Fase_TatuMafioso_01");
    }


}
