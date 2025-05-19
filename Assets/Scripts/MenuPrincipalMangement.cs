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

    private System.Action acaoConfirmada;
    public void Jogar()
    {
        SceneManager.LoadScene("Tutorial");
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
}
