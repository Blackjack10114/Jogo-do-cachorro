using UnityEngine;

public class Buraco : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // Se for o jogador, deixa o Playercheckpoint cuidar
        if (other.gameObject.CompareTag("Player")) return;

        // Se for a caixa separada
        if (other.gameObject.CompareTag("caixa"))
        {
            CaixaSeparada separada = other.gameObject.GetComponent<CaixaSeparada>();
            if (separada != null)
            {
                separada.RetornarCaixaParaJogador();
            }
        }
    }
}
