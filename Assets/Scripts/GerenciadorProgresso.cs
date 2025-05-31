using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorDeJogo : MonoBehaviour
{
    public static GerenciadorDeJogo Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Desbloqueia fase no progresso
    public void DesbloquearFase(int fase)
    {
        PlayerPrefs.SetInt("Fase_" + fase, 1);
        PlayerPrefs.Save();
    }

    // Verifica se fase está desbloqueada
    public bool FaseEstaDesbloqueada(int fase)
    {
        return PlayerPrefs.GetInt("Fase_" + fase, fase <= 1 ? 1 : 0) == 1;
    }

    public void ConcluirTutorial()
    {
        PlayerPrefs.SetInt("TutorialConcluido", 1);
        PlayerPrefs.Save();
    }

    public bool TutorialConcluido()
    {
        return PlayerPrefs.GetInt("TutorialConcluido", 0) == 1;
    }

    public void ResetarProgresso()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    // Avança para a próxima fase com base na cena atual
    public void IrParaProximaFase()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        string proximaCena = "";

        switch (cenaAtual)
        {
            case "Tutorial":
                proximaCena = "Fase_TatuMafioso_01";
                DesbloquearFase(1);
                ConcluirTutorial();
                break;

            case "Fase_TatuMafioso_01":
                proximaCena = "Fase_Alien_02";
                DesbloquearFase(2);
                break;

            case "Fase_Alien_02":
                proximaCena = "Fase_Dino_03";
                DesbloquearFase(3);
                break;

            case "Fase_Dino_03":
                proximaCena = "MenuPrincipal";
                break;
        }

        if (!string.IsNullOrEmpty(proximaCena))
        {
            SceneManager.LoadScene(proximaCena);
        }
    }
}
