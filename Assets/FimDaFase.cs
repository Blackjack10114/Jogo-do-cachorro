using UnityEngine;

public class FimDaFase : MonoBehaviour
{
    private SistemaPontuacao pontuacaoScript;
    public GameObject canvasPontuacao; // arraste o Canvas no Inspector

    void Start()
    {
        pontuacaoScript = Object.FindFirstObjectByType<SistemaPontuacao>();
        canvasPontuacao.SetActive(false); // já começa invisível
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pontuacaoScript.CalcularPontuacaoFinal();  // Calcula a pontuação
            canvasPontuacao.SetActive(true);           // Mostra o Canvas
            Time.timeScale = 0f;                        // PARA o tempo do jogo
        }
    }
}
