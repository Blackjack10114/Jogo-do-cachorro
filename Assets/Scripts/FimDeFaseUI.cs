using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FimDeFaseUI : MonoBehaviour
{
    public Text textoPontuacao;
    public Text textoNota;
    public EstrelaPontuacaoUI estrelasUI;

    void Start()
    {
        int pontos = PlayerPrefs.GetInt("PontuacaoNumerica", 0);
        string nota = PlayerPrefs.GetString("NotaFinal", "F");

        textoPontuacao.text = "Pontuação: " + pontos;
        textoNota.text = "Nota: " + nota;

        estrelasUI.AtualizarEstrelasNota(nota);
    }
    void ReiniciarFase()
    {
        // Opcional: reseta o tempo do jogo (caso ele tenha sido pausado)
        Time.timeScale = 1f;

        // Carrega de volta a cena principal (coloque o nome correto)
        SceneManager.LoadScene("SampleSceneHugo");
    }
}
