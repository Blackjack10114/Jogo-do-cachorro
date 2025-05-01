using UnityEngine;

public class PlataformaReativa : MonoBehaviour
{
    public float tempoAtiva = 2f;
    public float tempoInativa = 2f;
    public Sprite spriteComEspinhos;
    public Sprite spriteSemEspinhos;

    public GameObject visualObj; // objeto filho com SpriteRenderer

    private SpriteRenderer sr;
    private Collider2D colisor;
    private bool ativa = false;
    private float timer;

    void Start()
    {
        if (visualObj != null)
        {
            sr = visualObj.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogWarning("Objeto visual não atribuído!");
        }

        colisor = GetComponent<Collider2D>();
        timer = tempoInativa;
        SetEstado(false); // começa inativa
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SetEstado(!ativa); // alterna estado
            timer = ativa ? tempoAtiva : tempoInativa;
        }
    }

    void SetEstado(bool estadoAtivo)
    {
        ativa = estadoAtivo;

        if (ativa)
        {
            gameObject.tag = "Spike";
            if (sr != null && spriteComEspinhos != null)
                sr.sprite = spriteComEspinhos;
        }
        else
        {
            gameObject.tag = "PlataformaReativa";
            if (sr != null && spriteSemEspinhos != null)
                sr.sprite = spriteSemEspinhos;
        }
    }
}
