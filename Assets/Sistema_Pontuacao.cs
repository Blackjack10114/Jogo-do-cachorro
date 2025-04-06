using UnityEngine;
using UnityEngine.UI;


public class SistemaPontuacao : MonoBehaviour
{
    public TempoFase tempoScript;    // Refer�ncia ao script que controla o tempo
    public Dano danoScript;          // Refer�ncia ao script que cont�m a vida da caixa
    public int vidaMaxima = 3;       // M�xima vida poss�vel da caixa
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

        // B�nus baseado na velocidade (quanto mais r�pido que a meta, maior o b�nus)
        float bonusTempo = 0f;
        if (tempoFinal < tempoMeta)
        {
            float proporcaoTempo = 1f - (tempoFinal / tempoMeta);
            bonusTempo = Mathf.RoundToInt(100 * proporcaoTempo); // b�nus at� 100 pontos
        }

        // Pontua��o base (valor fixo que ser� multiplicado)
        int basePontos = 500;

        // C�lculo final da pontua��o
        pontuacaoFinal = Mathf.RoundToInt(basePontos * multiplicadorVida + bonusTempo);

        // Exibe no console a pontua��o calculada
        textoPontuacao.text = "Pontua��o Final: " + pontuacaoFinal;

    }
}
