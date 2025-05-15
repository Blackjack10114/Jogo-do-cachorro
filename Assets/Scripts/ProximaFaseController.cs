using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProximaFaseController : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private Button botaoProximaFase;
    [SerializeField] private string[] fasesDaDemo = { "Fase_Playtest", "Fase_Alien_02" };

    [Header("Aparência do Botão")]
    [SerializeField] private Color corCompleto = Color.gray;
    [SerializeField] private string textoCompleto = "Demo Concluída!";

    void Start()
    {
        if (botaoProximaFase != null)
        {
            botaoProximaFase.onClick.AddListener(CarregarProximaFase);
            AtualizarBotao();
        }
    }

    public void CarregarProximaFase()
    {
        int proximaFaseIndex = PlayerPrefs.GetInt("FaseCompleta", -1) + 1;

        if (proximaFaseIndex < fasesDaDemo.Length)
        {
            SceneManager.LoadScene(fasesDaDemo[proximaFaseIndex]);
        }
    }

    public static void RegistrarFaseCompleta()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        int faseAtual = -1;

        if (cenaAtual == "Fase_Playtest") faseAtual = 0;
        else if (cenaAtual == "Fase_Alien_02") faseAtual = 1;

        if (faseAtual > PlayerPrefs.GetInt("FaseCompleta", -1))
        {
            PlayerPrefs.SetInt("FaseCompleta", faseAtual);
            PlayerPrefs.Save();
        }
    }

    private void AtualizarBotao()
    {
        bool todasCompletas = PlayerPrefs.GetInt("FaseCompleta", -1) >= fasesDaDemo.Length - 1;

        botaoProximaFase.interactable = !todasCompletas;

        if (todasCompletas)
        {
            var textComponent = botaoProximaFase.GetComponentInChildren<Text>();
            if (textComponent != null) textComponent.text = textoCompleto;
            botaoProximaFase.image.color = corCompleto;
        }
    }

    // Método para testes (opcional)
    [ContextMenu("Resetar Progresso")]
    public void ResetarProgresso()
    {
        PlayerPrefs.DeleteKey("FaseCompleta");
        AtualizarBotao();
        Debug.Log("Progresso das fases resetado!");
    }
}