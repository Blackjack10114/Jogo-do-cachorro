using UnityEngine;
using UnityEngine.UI;


public class SistemaPontuacao : MonoBehaviour
{
    public TempoFase tempoScript;    // Referência ao script que controla o tempo
    public Dano danoScript;          // Referência ao script que contém a vida da caixa
    public int vidaMaxima = 3;       // Máxima vida possível da caixa
    public float tempoMeta = 120f;   // Meta de tempo em segundos
    public Text textoPontuacao;


    private int pontuacaoFinal;

    public void CalcularPontuacaoFinal()
    {
        // Pega a vida atual da caixa diretamente do script Dano
        float vidaAtual = danoScript.pv;

        // Pega o tempo final da fase
        float tempoFinal = tempoScript.tempoAtual;

        // Multiplicador baseado na vida restante (escala entre 0 e 1)
        float multiplicadorVida = vidaAtual / vidaMaxima;

        // Bônus baseado na velocidade (quanto mais rápido que a meta, maior o bônus)
        float bonusTempo = 0f;
        if (tempoFinal < tempoMeta)
        {
            float proporcaoTempo = 1f - (tempoFinal / tempoMeta);
            bonusTempo = Mathf.RoundToInt(100 * proporcaoTempo); // bônus até 100 pontos
        }

        // Pontuação base (valor fixo que será multiplicado)
        int basePontos = 500;

        // Cálculo final da pontuação
        pontuacaoFinal = Mathf.RoundToInt(basePontos * multiplicadorVida + bonusTempo);

        // Exibe no console a pontuação calculada
        textoPontuacao.text = "Pontuação Final: " + pontuacaoFinal;

    }
}
