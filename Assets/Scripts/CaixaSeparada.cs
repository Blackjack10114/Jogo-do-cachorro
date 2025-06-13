using UnityEngine;

public class CaixaSeparada : MonoBehaviour
{
    private Caixa caixaOriginal;

    public void SetOrigem(Caixa origem)
    {
        caixaOriginal = origem;
    }

    public void RetornarCaixaParaJogador()
    {
        if (caixaOriginal != null)
        {
            caixaOriginal.ForcarRetornoCaixaDoBuraco();
        }

        Destroy(gameObject);
    }
}
