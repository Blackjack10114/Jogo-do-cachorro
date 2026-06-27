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
    public Text textoEntrega; // Texto que exibe "Entrega Concluída" ou "Entrega Falhou"
    public string nota;
    public float tempo;

    void Start()
    {
        int entregaFalhou = PlayerPrefs.GetInt("EntregaFalhou", 0);

        if (entregaFalhou == 1)
        {
            // Mostra mensagem de falha
            if (textoEntrega != null)
                textoEntrega.text = "ENTREGA FALHOU!";

            // Esconde pontuação
            if (textoNota != null) textoNota.text = "";
            if (textoPontuacao != null) textoPontuacao.text = "";
            if (textoBonus != null) textoBonus.text = "";
            if (textoBonusTempo != null) textoBonusTempo.text = "";
            if (textoBonusVida != null) textoBonusVida.text = "";
            if (textoBonusOssos != null) textoBonusOssos.text = "";
            if (textoPenalidade != null) textoPenalidade.text = "";
            if (textoTempoFinal != null) textoTempoFinal.text = "";
            if (textoVidaFinal != null) textoVidaFinal.text = "";
            if (textoQuantidadeOssos != null) textoQuantidadeOssos.text = "";
            if (textoColisoes != null) textoColisoes.text = "";

            if (estrelasUI != null)
                estrelasUI.AtualizarEstrelas(0f);

            PlayerPrefs.DeleteKey("EntregaFalhou"); // Reseta flag
            return;
        }

        // Caso entrega tenha sido concluída
        if (textoEntrega != null)
            textoEntrega.text = "ENTREGA CONCLUÍDA!";

        tempo = PlayerPrefs.GetFloat("TempoFinal", 0f);
        float vida = PlayerPrefs.GetFloat("VidaFinal", 0f);
        int ossos = PlayerPrefs.GetInt("OssosColetados", 0);
        int bonusTempo = PlayerPrefs.GetInt("BonusTempo", 0);
        int bonusVida = PlayerPrefs.GetInt("BonusVida", 0);
        int pontosOssos = PlayerPrefs.GetInt("PontosOssos", 0);
        int penalidade = PlayerPrefs.GetInt("Penalidades", 0);
        int pontos = PlayerPrefs.GetInt("PontuacaoNumerica", 0);
        nota = PlayerPrefs.GetString("ClassificacaoLetra", "F");

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
            textoTempoFinal.text = $"{tempo:F2}s";

        if (textoVidaFinal != null)
            textoVidaFinal.text = $"{vida:F0}%";

        if (textoQuantidadeOssos != null)
            textoQuantidadeOssos.text = $"{ossos}/3";

        if (textoPontuacao != null)
            textoPontuacao.text = "" + pontos;

        if (textoNota != null)
            textoNota.text = "" + nota;

        if (estrelasUI != null)
            estrelasUI.AtualizarEstrelasNota(nota);
            }

    void ReiniciarFase()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(ProximaFaseController.ObterCenaAtualRegistrada());
    }
}
