using UnityEngine;
using UnityEngine.UI;

public class EstrelaPontuacaoUI : MonoBehaviour
{
    public GameObject estrelaPrefab; // Prefab com um Image
    public Sprite estrelaVazia;
    public Sprite estrelaMeia;
    public Sprite estrelaCheia;
    public Sprite estrelaVermelha; // Sprite para S+
    public Transform containerEstrelas;

    private Image[] estrelas;

    void Start()
    {
        CriarEstrelas();
        string nota = PlayerPrefs.GetString("NotaFinal", "F");
        AtualizarEstrelasNota(nota);
    }

    void CriarEstrelas()
    {
        estrelas = new Image[5];
        for (int i = 0; i < 5; i++)
        {
            GameObject novaEstrela = Instantiate(estrelaPrefab, containerEstrelas);
            if (novaEstrela == null)
            {
                Debug.LogError($"❌ Falha ao instanciar o prefab da estrela na posição {i}.");
                continue;
            }

            Image estrelaImg = novaEstrela.GetComponent<Image>();
            if (estrelaImg == null)
            {
                estrelaImg = novaEstrela.GetComponentInChildren<Image>();
            }

            if (estrelaImg == null)
            {
                Debug.LogError($"❌ Prefab da estrela instanciada ({novaEstrela.name}) não contém Image.");
                continue;
            }

            estrelas[i] = estrelaImg;
        }
    }



    public void AtualizarEstrelasNota(string nota)
    {
        if (nota == "S+")
        {
            // Todas vermelhas
            foreach (Image estrela in estrelas)
            {
                estrela.sprite = estrelaVermelha;
            }
        }
        else
        {
            float estrelasFloat = NotaParaEstrelas(nota);
            AtualizarEstrelas(estrelasFloat);
        }
    }

    public void AtualizarEstrelas(float estrelasFloat)
    {
        if (estrelas == null || estrelas.Length == 0)
        {
            Debug.LogWarning("Todas estrelas foram criadas");
            return;
        }

        if (estrelasFloat >= 6f)
        {
            for (int i = 0; i < estrelas.Length; i++)
            {
                if (estrelas[i] != null)
                    estrelas[i].sprite = estrelaVermelha;
            }
            return;
        }

        for (int i = 0; i < estrelas.Length; i++)
        {
            if (estrelas[i] == null) continue;

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
            case "S+": return 6f;
            case "S": return 5f;
            case "A+": return 4.5f;
            case "A": return 4f;
            case "A-": return 3.5f;
            case "B+": return 3f;
            case "B": return 2.5f;
            case "C+": return 2f;
            case "C": return 1.5f;
            case "F": return 1f;
            default: return 0f;
        }
    }

}

