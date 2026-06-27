using UnityEngine;
using UnityEngine.UI;

public class Tempopowerup : MonoBehaviour
{
    public enum TipoPowerUp
    {
        Turbo,
        Gourmet,
        PuloDuplo
    }

    public TipoPowerUp tipo;

    private PlayerMov player;
    private Caixa caixa;
    private Text texto;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMov>();
        caixa = player.GetComponent<Caixa>();
        texto = GetComponent<Text>();
    }

    void Update()
    {
        switch (tipo)
        {
            case TipoPowerUp.Turbo:
                Atualizar(player.isTurboActive, player.turboTimer);
                break;

            case TipoPowerUp.Gourmet:
                Atualizar(player.isGourmetActive, player.gourmetTimer);
                break;

            case TipoPowerUp.PuloDuplo:
                Atualizar(player.temPuloDuplo, caixa.DuracaoPuloDuplo);
                break;
        }
    }

    void Atualizar(bool ativo, float tempo)
    {
        if (!ativo)
        {
            Destroy(gameObject);
            return;
        }

        texto.text = Mathf.CeilToInt(tempo).ToString();
    }
}
