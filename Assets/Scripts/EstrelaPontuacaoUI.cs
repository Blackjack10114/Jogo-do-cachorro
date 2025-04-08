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

            // Tenta pegar o Image diretamente no objeto
            Image estrelaImg = novaEstrela.GetComponent<Image>();

            // Se não achou no objeto principal, procura nos filhos
            if (estrelaImg == null)
            {
                estrelaImg = novaEstrela.GetComponentInChildren<Image>();
            }

            if (estrelaImg == null)
            {
                Debug.LogError($"Não foi encontrado um componente Image dentro da estrela instanciada ({novaEstrela.name})!");
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
        if (estrelasFloat >= 6f)
        {
            // S+ → todas as 5 estrelas vermelhas
            for (int i = 0; i < estrelas.Length; i++)
            {
                estrelas[i].sprite = estrelaVermelha;
            }
            return;
        }

        // S ou menor → calcula como antes
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

