using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FimDeFaseUI : MonoBehaviour
{
    public Text textoPontuacao;
    public Text textoNota;
    public EstrelaPontuacaoUI estrelasUI;
    public Text textoBonus;
    public Text textoBonusTempo;
    public Text textoBonusVida;
    public Text textoBonusOssos;
    public Text textoColisoes;
    public Text textoPenalidade; 

    void Start()
    {
        int bonusTempo = PlayerPrefs.GetInt("BonusTempo", 0);
        int bonusVida = PlayerPrefs.GetInt("BonusVida", 0);
        int pontosOssos = PlayerPrefs.GetInt("PontosOssos", 0);
        int penalidade = PlayerPrefs.GetInt("Penalidades", 0);

        if (textoBonus != null)
            textoBonus.text = $"Bônus Total: {bonusTempo + bonusVida}";

        if (textoBonusTempo != null)
            textoBonusTempo.text = $"Bônus por Tempo: {bonusTempo}";

        if (textoBonusVida != null)
            textoBonusVida.text = $"Bônus por Vida: {bonusVida}";

        if (textoBonusOssos != null)
            textoBonusOssos.text = $"Pontos por Ossos: {pontosOssos}";

        if (textoPenalidade != null)
        {
            if (penalidade > 0)
                textoPenalidade.text = $"Penalidade por Colisões: -{penalidade}";
            else
                textoPenalidade.text = "Sem penalidades!";
        }


        int pontos = PlayerPrefs.GetInt("PontuacaoNumerica", 0);
        string nota = PlayerPrefs.GetString("ClassificacaoLetra", "F");

        textoPontuacao.text = "Pontuação: " + pontos;
        textoNota.text = "Nota: " + nota;

        estrelasUI.AtualizarEstrelasNota(nota);
    }

    void ReiniciarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleSceneHugo");
    }
}