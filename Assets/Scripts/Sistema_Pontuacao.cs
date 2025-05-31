using UnityEngine;
using UnityEngine.UI;

public class SistemaPontuacao : MonoBehaviour
{
    public TempoFase tempoScript;
    public Dano danoScript;

    public int vidaMaxima = 100;
    public float tempoMeta;

    public int ossosColetados = 0;
    public int numeroColisoes = 0;

    public int penalidadePorColisao = 3;
    public int pontosPorOsso = 200;

    public int totalDeOssosDaFase = 3;

    public Text textoPontuacao;

    private int pontuacaoNumerica;
    private float pontuacaoEstrelas;
    private string classificacaoLetra;

    private int bonusVida = 0;
    private int bonusTempo = 0;
    private int bonusTotal = 0;

    public void AdicionarOsso() => ossosColetados++;
    

    void Start()
    {
        if (danoScript != null)
        {
            vidaMaxima = Mathf.RoundToInt(danoScript.pv);
        }
    }

    public void CalcularPontuacaoFinal()
    {
        float vidaAtual = danoScript.pv;
        float tempoFinal = tempoScript.tempoAtual;

        // BÔNUS POR VIDA 
        float proporcaoVida = Mathf.Clamp01(vidaAtual / vidaMaxima);
        bonusVida = Mathf.RoundToInt(600 * proporcaoVida);

        // BÔNUS POR OSSO
        int pontosOssos = ossosColetados * pontosPorOsso;

        
        // BÔNUS POR TEMPO
        bonusTempo = 0;
        if (tempoFinal < tempoMeta)
        {
            float proporcaoTempo = 1f - (tempoFinal / tempoMeta);
            bonusTempo = Mathf.RoundToInt(1000 * proporcaoTempo);
        }

        bonusTotal = bonusVida + bonusTempo;

        pontuacaoNumerica = bonusVida + pontosOssos + bonusTempo;
        pontuacaoNumerica = Mathf.Max(0, pontuacaoNumerica);

        int pontuacaoMaxima = 400 + 400 + (totalDeOssosDaFase * pontosPorOsso);
        float proporcaoEstrelas = (float)pontuacaoNumerica / pontuacaoMaxima;
        pontuacaoEstrelas = Mathf.Round(proporcaoEstrelas * 10f) / 2f;

        // Classificação com intermediários
        if (pontuacaoNumerica >= 950 && ossosColetados == totalDeOssosDaFase && vidaAtual >= 95)
            classificacaoLetra = "S+";
        else if (pontuacaoNumerica >= 850)
            classificacaoLetra = "S";
        else if (pontuacaoNumerica >= 800)
            classificacaoLetra = "A+";
        else if (pontuacaoNumerica >= 750)
            classificacaoLetra = "A";
        else if (pontuacaoNumerica >= 700)
            classificacaoLetra = "A-";
        else if (pontuacaoNumerica >= 650)
            classificacaoLetra = "B+";
        else if (pontuacaoNumerica >= 600)
            classificacaoLetra = "B";
        else if (pontuacaoNumerica >= 550)
            classificacaoLetra = "C+";
        else if (pontuacaoNumerica >= 500)
            classificacaoLetra = "C";
        else
            classificacaoLetra = "F";

        PlayerPrefs.SetFloat("PontuacaoFinal", pontuacaoEstrelas);
        PlayerPrefs.SetInt("PontuacaoNumerica", pontuacaoNumerica);
        PlayerPrefs.SetString("ClassificacaoLetra", classificacaoLetra);
        PlayerPrefs.SetInt("BonusVida", bonusVida);
        PlayerPrefs.SetInt("BonusTempo", bonusTempo);
        PlayerPrefs.SetInt("BonusTotal", bonusTotal);
        PlayerPrefs.SetInt("PontosOssos", pontosOssos);      
        PlayerPrefs.SetFloat("TempoFinal", tempoFinal);
        PlayerPrefs.SetFloat("VidaFinal", vidaAtual);
        PlayerPrefs.SetInt("OssosColetados", ossosColetados);
        PlayerPrefs.Save();

        if (textoPontuacao != null)
        {
            textoPontuacao.text = $"Pontuação Final: {pontuacaoNumerica} ({pontuacaoEstrelas} estrelas, Nota {classificacaoLetra})";
        }
        bool falhou = classificacaoLetra == "F";
       // Controlador_Som.instancia.DefinirMusicaFimDeFase(falhou);

    }

    public float GetPontuacaoFinal() => pontuacaoEstrelas;
    public int GetPontuacaoFinalNumerica() => pontuacaoNumerica;
    public float GetPontuacaoEstrelas() => pontuacaoEstrelas;
    public string GetClassificacaoLetra() => classificacaoLetra;
}
