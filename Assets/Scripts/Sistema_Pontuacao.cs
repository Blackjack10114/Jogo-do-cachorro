using UnityEngine;
using UnityEngine.UI;

public class SistemaPontuacao : MonoBehaviour
{
    public TempoFase tempoScript;
    public Dano danoScript;

    public int vidaMaxima = 100;
    public float tempoMeta = 120f;

    public int ossosColetados = 0;
    public int numeroColisoes = 0;

    public int penalidadePorColisao = 30;
    public int pontosPorOsso = 50;

    public int totalDeOssosDaFase = 3;

    public Text textoPontuacao;

    private int pontuacaoNumerica;
    private float pontuacaoEstrelas;
    private string classificacaoLetra;

    public void AdicionarOsso()
    {
        ossosColetados++;
    }

    public void AdicionarColisao()
    {
        numeroColisoes++;
    }

    public void CalcularPontuacaoFinal()
    {
        float vidaAtual = danoScript.pv;
        float tempoFinal = tempoScript.tempoAtual;

        float multiplicadorVida = Mathf.Clamp01(vidaAtual / vidaMaxima);
        int pontosVida = Mathf.RoundToInt(500 * multiplicadorVida);
        int pontosOssos = ossosColetados * pontosPorOsso;
        int penalidade = numeroColisoes * penalidadePorColisao;

        int bonusTempo = 0;
        if (tempoFinal < tempoMeta)
        {
            float proporcaoTempo = 1f - (tempoFinal / tempoMeta);
            bonusTempo = Mathf.RoundToInt(400 * proporcaoTempo);
        }

        pontuacaoNumerica = pontosVida + pontosOssos + bonusTempo - penalidade;
        pontuacaoNumerica = Mathf.Max(0, pontuacaoNumerica);

        int pontuacaoMaxima = 500 + 400 + (totalDeOssosDaFase * pontosPorOsso);
        float proporcaoEstrelas = (float)pontuacaoNumerica / pontuacaoMaxima;
        pontuacaoEstrelas = Mathf.Round(proporcaoEstrelas * 10f) / 2f;

        // 🟩 Checagem especial para S+
        if (
            pontuacaoNumerica >= 950 &&
            ossosColetados == totalDeOssosDaFase &&
            numeroColisoes <= 1 &&
            vidaAtual >= 90
        )
        {
            classificacaoLetra = "S+";
        }
        else if (pontuacaoNumerica >= 850) classificacaoLetra = "S";
        else if (pontuacaoNumerica >= 750) classificacaoLetra = "A";
        else if (pontuacaoNumerica >= 600) classificacaoLetra = "B";
        else if (pontuacaoNumerica >= 400) classificacaoLetra = "C";
        else classificacaoLetra = "F";

        PlayerPrefs.SetFloat("PontuacaoFinal", pontuacaoEstrelas);
        PlayerPrefs.SetInt("PontuacaoNumerica", pontuacaoNumerica);
        PlayerPrefs.SetString("ClassificacaoLetra", classificacaoLetra);
        PlayerPrefs.Save();

        if (textoPontuacao != null)
        {
            textoPontuacao.text = $"Pontuação Final: {pontuacaoNumerica} ({pontuacaoEstrelas} estrelas, Nota {classificacaoLetra})";
        }
    }

    // Métodos auxiliares
    public float GetPontuacaoFinal() => pontuacaoEstrelas;
    public int GetPontuacaoFinalNumerica() => pontuacaoNumerica;
    public float GetPontuacaoEstrelas() => pontuacaoEstrelas;
    public string GetClassificacaoLetra() => classificacaoLetra;
}
