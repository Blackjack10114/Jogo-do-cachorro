using UnityEngine;
using UnityEngine.UI;

public class SistemaPontuacao : MonoBehaviour
{
    public TempoFase tempoScript;
    public Dano danoScript;

    public int vidaMaxima = 3;
    public float tempoMeta = 120f;

    public int ossosColetados = 0;
    public int numeroColisoes = 0;

    public int penalidadePorColisao = 30;
    public int pontosPorOsso = 50;

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

        float multiplicadorVida = vidaAtual / vidaMaxima;

        float bonusTempo = 0f;
        if (tempoFinal < tempoMeta)
        {
            float proporcaoTempo = 1f - (tempoFinal / tempoMeta);
            bonusTempo = Mathf.RoundToInt(100 * proporcaoTempo);
        }

        int basePontos = 500;
        int pontosOssos = ossosColetados * pontosPorOsso;
        int penalidade = numeroColisoes * penalidadePorColisao;

        pontuacaoNumerica = Mathf.RoundToInt(basePontos * multiplicadorVida + bonusTempo + pontosOssos - penalidade);
        pontuacaoNumerica = Mathf.Max(0, pontuacaoNumerica); // Garante que não seja negativo

        int pontuacaoMaxima = basePontos + 100 + (ossosColetados * pontosPorOsso);
        float proporcaoEstrelas = (float)pontuacaoNumerica / pontuacaoMaxima;
        pontuacaoEstrelas = Mathf.Round(proporcaoEstrelas * 10f) / 2f; // Arredonda para 0.5 em 0.5

        // Classificação por letra
        if (pontuacaoNumerica >= 1000) classificacaoLetra = "S+";
        else if (pontuacaoNumerica >= 800) classificacaoLetra = "A";
        else if (pontuacaoNumerica >= 600) classificacaoLetra = "B";
        else if (pontuacaoNumerica >= 400) classificacaoLetra = "C";
        else classificacaoLetra = "D";

        // Salva dados para a próxima cena
        PlayerPrefs.SetFloat("PontuacaoFinal", pontuacaoEstrelas);
        PlayerPrefs.SetInt("PontuacaoNumerica", pontuacaoNumerica);
        PlayerPrefs.SetString("ClassificacaoLetra", classificacaoLetra);
        PlayerPrefs.Save();

        // Mostra no UI se o Text estiver ligado
        if (textoPontuacao != null)
        {
            textoPontuacao.text = $"Pontuação Final: {pontuacaoNumerica} ({pontuacaoEstrelas} estrelas, Nota {classificacaoLetra})";
        }
    }

    // Métodos auxiliares para acessar de fora
    public float GetPontuacaoFinal() => pontuacaoEstrelas;
    public int GetPontuacaoFinalNumerica() => pontuacaoNumerica;
    public float GetPontuacaoEstrelas() => pontuacaoEstrelas;
    public string GetClassificacaoLetra() => classificacaoLetra;
}
