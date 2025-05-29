using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeFase : MonoBehaviour
{
    public void IrParaProximaFase()
    {
        string faseAtual = SceneManager.GetActiveScene().name;
        string proximaFase = "";

        switch (faseAtual)
        {
            case "Tutorial":
                proximaFase = "Fase_TatuMafioso_01";
                GerenciadorDeProgresso.Instance.DesbloquearFase(1);
                GerenciadorDeProgresso.Instance.ConcluirTutorial();
                break;
            case "Fase_TatuMafioso_01":
                proximaFase = "Fase_Alien_02";
                GerenciadorDeProgresso.Instance.DesbloquearFase(2);
                break;
            case "Fase_Alien_02":
                proximaFase = "Fase_Dino_03";
                GerenciadorDeProgresso.Instance.DesbloquearFase(3);
                break;
            case "Fase_Dino_03":
                proximaFase = "MenuPrincipal"; // Volta pro menu
                break;
        }

        if (proximaFase != "")
        {
            SceneManager.LoadScene(proximaFase);
        }
    }
}
