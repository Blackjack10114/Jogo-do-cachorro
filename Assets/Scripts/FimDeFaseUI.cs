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
    public Text textoTempoFinal;
    public Text textoVidaFinal;
    public Text textoQuantidadeOssos;



    void Start()
    {
        float tempo = PlayerPrefs.GetFloat("TempoFinal", 0f);
        float vida = PlayerPrefs.GetFloat("VidaFinal", 0f);

        int ossos = PlayerPrefs.GetInt("OssosColetados", 0);
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

        if (textoTempoFinal != null)
            textoTempoFinal.text = $"Tempo Final: {tempo:F2}s";

        if (textoVidaFinal != null)
            textoVidaFinal.text = $"Vida da Caixa: {vida:F0}%";

        if (textoQuantidadeOssos != null)
            textoQuantidadeOssos.text = $"Ossos Coletados: {ossos}/3";

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