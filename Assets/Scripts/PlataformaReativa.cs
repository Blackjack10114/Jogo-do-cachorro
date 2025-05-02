using UnityEngine;

public class PlataformaReativa : MonoBehaviour
{
    public float tempoAtiva = 2f;
    public float tempoInativa = 2f;

    // Para prefab com objetos filhos
    public GameObject objetoEspinho;     // Triângulo (ativado/desativado)
    public GameObject objetoPlataforma;  // Quadrado (plataforma base)

    // Para prefab com SpriteRenderer
    public GameObject visualObj;                // Objeto com SpriteRenderer (opcional)
    public Sprite spriteComEspinhos;            // Sprite com espinhos
    public Sprite spriteSemEspinhos;            // Sprite sem espinhos

    private SpriteRenderer sr;
    private Collider2D colisor;
    private bool ativa = false;
    private float timer;

    void Start()
    {
        colisor = GetComponent<Collider2D>();
        if (visualObj != null)
            sr = visualObj.GetComponent<SpriteRenderer>();

        timer = tempoInativa;
        SetEstado(false); // começa desativada
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SetEstado(!ativa);
            timer = ativa ? tempoAtiva : tempoInativa;
        }
    }

    void SetEstado(bool estadoAtivo)
    {
        ativa = estadoAtivo;

        // Altera a tag do objeto principal
        gameObject.tag = ativa ? "Spike" : "PlataformaReativa";

        // Ativa/Desativa os objetos filhos
        if (objetoEspinho != null)
            objetoEspinho.SetActive(ativa); // ativa espinho apenas quando "ativa"

        if (objetoPlataforma != null)
            objetoPlataforma.SetActive(true); // sempre visível (ou também controlar se quiser)

        // Opcional: troca sprites, se for usar spriteRenderer futuramente
        if (sr != null)
        {
            sr.sprite = ativa ? spriteComEspinhos : spriteSemEspinhos;
        }
    }

}
