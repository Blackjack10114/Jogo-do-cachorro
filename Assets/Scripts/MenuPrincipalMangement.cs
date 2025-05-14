using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class MenuPrincipalMangement : MonoBehaviour
{
    [SerializeField]private string nomeDoLevelDeJogo;
    [SerializeField]private GameObject painelMenuInicial;
    [SerializeField] private GameObject painelOpcoes;
   public void Jogar()
    {
        SceneManager.LoadScene(nomeDoLevelDeJogo);
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
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
    
    public void ProximaFase()
    {
        SceneManager.LoadScene("Fase_Alien_02");
    }
}
