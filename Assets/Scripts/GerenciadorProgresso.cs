using UnityEngine;

public class GerenciadorDeProgresso : MonoBehaviour
{
    public static GerenciadorDeProgresso Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Não destrói ao trocar de cena
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Salva qual fase está
    public void DesbloquearFase(int fase)
    {
        PlayerPrefs.SetInt("Fase_" + fase, 1);
        PlayerPrefs.Save();
    }

    // Verifica se fase está desbloqueada
    public bool FaseEstaDesbloqueada(int fase)
    {
        return PlayerPrefs.GetInt("Fase_" + fase, fase <= 1 ? 1 : 0) == 1;
        // Fase do Tatu sempre desbloqueada
    }

    // Marcar Tutorial como concluído
    public void ConcluirTutorial()
    {
        PlayerPrefs.SetInt("TutorialConcluido", 1);
        PlayerPrefs.Save();
    }

    public bool TutorialConcluido()
    {
        return PlayerPrefs.GetInt("TutorialConcluido", 0) == 1;
    }

    // Reseta progresso para playtest ou rejogar
    public void ResetarProgresso()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
