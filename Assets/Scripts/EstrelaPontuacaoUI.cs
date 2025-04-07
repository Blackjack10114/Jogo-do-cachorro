using UnityEngine;
using UnityEngine.UI;

public class EstrelaPontuacaoUI : MonoBehaviour
{
    public Image[] estrelas; // Arraste as 3 imagens de estrela no inspetor
    public Sprite estrelaVazia;
    public Sprite estrelaMeia;
    public Sprite estrelaCheia;

    public void AtualizarEstrelasNota(string nota)
    {
        float estrelasFloat = NotaParaEstrelas(nota);
        AtualizarEstrelas(estrelasFloat);
    }

    public void AtualizarEstrelas(float estrelasFloat)
    {
        for (int i = 0; i < estrelas.Length; i++)
        {
            float valor = estrelasFloat - i;

            if (valor >= 1f)
            {
                estrelas[i].sprite = estrelaCheia;
            }
            else if (valor >= 0.5f)
            {
                estrelas[i].sprite = estrelaMeia;
            }
            else
            {
                estrelas[i].sprite = estrelaVazia;
            }
        }
    }

    private float NotaParaEstrelas(string nota)
    {
        switch (nota)
        {
            case "S+": return 5f;
            case "S": return 4f;
            case "A": return 3.5f;
            case "B": return 3f;
            case "C": return 2.5f;
            case "F": return 1f;
            default: return 0f;
        }
    }
}
