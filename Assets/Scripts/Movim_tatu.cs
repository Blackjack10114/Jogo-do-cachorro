using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Movim_tatu : MonoBehaviour
{
    [Header("Configuração")]
    public float speed = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool indoParaEsquerda = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        // Evita atrito que poderia travar ou desacelerar o tatu
        if (rb.sharedMaterial == null)
        {
            var mat = new PhysicsMaterial2D("TatuMaterial")
            {
                friction = 0f,
                bounciness = 0f
            };
            rb.sharedMaterial = mat;
        }

        // Faz o tatu ser imune a empurrões do Player
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
        rb.mass = 1000f; // massa bem alta, quase impossível o Player empurrar
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        float direcao = indoParaEsquerda ? -1f : 1f;
        rb.linearVelocity = new Vector2(direcao * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") ||
            collision.gameObject.CompareTag("Walld") ||
            collision.gameObject.CompareTag("Spike") ||
            collision.gameObject.CompareTag("Tatu") ||
            collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("caixa"))
        {
            // pega o primeiro ponto de contato
            ContactPoint2D contact = collision.contacts[0];

            // diferença entre ponto de contato e centro do tatu
            float direcaoColisao = contact.point.x - transform.position.x;

            // Se o contato foi do lado que ele ESTÁ andando → troca direção
            if (indoParaEsquerda && direcaoColisao < 0)
            {
                MudarDirecao();
            }
            else if (!indoParaEsquerda && direcaoColisao > 0)
            {
                MudarDirecao();
            }
            // Se o contato veio do lado oposto → ignora
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // verifica a posição relativa do player em relação ao tatu
                float lado = other.transform.position.x - transform.position.x;

                // define a direção de empurrão: esquerda ou direita
                Vector2 direcao = lado < 0 ? new Vector2(-1f, 1f) : new Vector2(1f, 1f);

                // aplica força diagonal
                playerRb.AddForce(direcao.normalized * 80f, ForceMode2D.Impulse);
            }
        }
    }

    private void MudarDirecao()
    {
        indoParaEsquerda = !indoParaEsquerda;
        sprite.flipX = !indoParaEsquerda;
    }
}
