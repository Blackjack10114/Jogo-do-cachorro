using UnityEngine;
using System.Collections;

public class Fantasma : MonoBehaviour
{
    private enum EstadoFantasma
    {
        Invisivel,
        Aparecendo,
        Visivel,
        Piscando
    }

    private BoxCollider2D boxCollider;
    private SpriteRenderer sprite;

    [Header("Tempos")]
    [SerializeField] private float tempoInvisivel = 2f;
    [SerializeField] private float tempoAparecendo = 0.5f;
    [SerializeField] private float tempoVisivel = 2f;
    [SerializeField] private float tempoPiscando = 1f;

    [Header("Piscar")]
    [SerializeField] private float intervaloPisca = 0.15f;

    [Header("Spawn")]
    [SerializeField] private float raioReaparecer = 5f;

    [Header("Colisão")]
    [SerializeField] private float delayAtivarColisao = 0.3f;

    private EstadoFantasma estadoAtual;
    private Vector3 pontoInicial;
    private Transform player;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        pontoInicial = transform.position;
        boxCollider = GetComponent<BoxCollider2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Fantasma: Nenhum objeto com a tag 'Player' encontrado.");

        SetEstado(EstadoFantasma.Invisivel);
        StartCoroutine(CicloFantasma());
    }


    // =========================
    // CICLO PRINCIPAL
    // =========================
    IEnumerator CicloFantasma()
    {
        while (true)
        {
            // Invisível
            SetEstado(EstadoFantasma.Invisivel);
            boxCollider.enabled = false;
            yield return new WaitForSeconds(tempoInvisivel);

            Reposicionar();

            // Aparecendo
            SetEstado(EstadoFantasma.Aparecendo);
            boxCollider.enabled = false;
            yield return Fade(0f, 1f, tempoAparecendo);

            // Delay antes de poder colidir
            yield return new WaitForSeconds(delayAtivarColisao);
            boxCollider.enabled = true;

            // Visível (perigoso)
            SetEstado(EstadoFantasma.Visivel);
            yield return new WaitForSeconds(tempoVisivel);

            // Piscando antes de sumir
            SetEstado(EstadoFantasma.Piscando);
            yield return Piscar();

            SetAlpha(0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        // Desativa o próprio collider após atingir o player
        boxCollider.enabled = false;
    }


    // =========================
    // ESTADOS
    // =========================
    void SetEstado(EstadoFantasma novoEstado)
    {
        estadoAtual = novoEstado;

        switch (estadoAtual)
        {
            case EstadoFantasma.Invisivel:
                SetAlpha(0f);
                break;

            case EstadoFantasma.Aparecendo:
                break;

            case EstadoFantasma.Visivel:
                SetAlpha(1f);
                break;

            case EstadoFantasma.Piscando:
                break;
        }
    }

    // =========================
    // EFEITOS VISUAIS
    // =========================
    IEnumerator Fade(float inicio, float fim, float duracao)
    {
        float t = 0f;
        while (t < duracao)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(inicio, fim, t / duracao);
            SetAlpha(alpha);
            yield return null;
        }
        SetAlpha(fim);
    }

    IEnumerator Piscar()
    {
        float tempo = 0f;
        bool visivel = true;

        while (tempo < tempoPiscando)
        {
            visivel = !visivel;
            SetAlpha(visivel ? 1f : 0.2f);
            tempo += intervaloPisca;
            yield return new WaitForSeconds(intervaloPisca);
        }
    }

    void SetAlpha(float alpha)
    {
        Color cor = sprite.color;
        cor.a = alpha;
        sprite.color = cor;
    }

    // =========================
    // POSICIONAMENTO
    // =========================
    void Reposicionar()
    {
        Vector2 centro = player != null ? player.position : pontoInicial;
        Vector2 offset = Random.insideUnitCircle * raioReaparecer;
        transform.position = centro + offset;
    }
}