using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FimDeFaseUI : MonoBehaviour
{
    public Text textoPontuacao;       // Arraste aqui o Text da pontua��o
    public Button botaoReiniciar;     // Arraste o bot�o aqui pelo Inspector

    void Start()
    {
        // Pega a pontua��o salva
        int pontuacao = PlayerPrefs.GetInt("PontuacaoFinal", 0);
        textoPontuacao.text = "Pontua��o: " + pontuacao;

        // Adiciona o evento ao bot�o
        botaoReiniciar.onClick.AddListener(ReiniciarFase);
    }

    void ReiniciarFase()
    {
        // Opcional: reseta o tempo do jogo (caso ele tenha sido pausado)
        Time.timeScale = 1f;

        // Carrega de volta a cena principal (coloque o nome correto)
        SceneManager.LoadScene("SampleSceneHugo");
    }
}
