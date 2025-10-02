using UnityEngine;
using System.Collections;

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

        // Faz o tatu ser imune a empurrões do Player
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
        rb.mass = 9999f; 
        rb.gravityScale = 10f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        float direcao = indoParaEsquerda ? -1f : 1f;
        rb.linearVelocity = new Vector2(direcao * speed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;

        // empurra player ou caixa ---
        if (other.CompareTag("Player") || other.CompareTag("caixa"))
        {
            Rigidbody2D rbAlvo = other.GetComponent<Rigidbody2D>();
            if (rbAlvo != null)
            {
                float lado = other.transform.position.x - transform.position.x;

                // direções possíveis: empurrar sempre para DIAGONAL contrária
                Vector2 direcao = lado < 0 ? new Vector2(-1f, 0.3f) : new Vector2(1f, 0.3f);

                // se for player, dá um impulso mais forte
                float forca = other.CompareTag("Player") ? 100f : 80f;

                rbAlvo.AddForce(direcao.normalized * forca, ForceMode2D.Impulse);

                StartCoroutine(LimitarKnockback(rbAlvo));
            }
        }

        // --- mudar direção em colisões com "paredes/obstáculos" ---
        if (other.CompareTag("Wall") ||
            other.CompareTag("Walld") ||
            other.CompareTag("Spike") ||
            other.CompareTag("Tatu"))
        {
            ContactPoint2D contact = collision.contacts[0];
            float direcaoColisao = contact.point.x - transform.position.x;

            if (indoParaEsquerda && direcaoColisao < 0)
            {
                MudarDirecao();
            }
            else if (!indoParaEsquerda && direcaoColisao > 0)
            {
                MudarDirecao();
            }
        }
    }



    IEnumerator LimitarKnockback(Rigidbody2D alvo)
    {
        yield return new WaitForFixedUpdate();
        if (alvo.linearVelocity.y > 25f)
        {
            alvo.linearVelocity = new Vector2(alvo.linearVelocity.x, 25f);
        }
    }

    private void MudarDirecao()
    {
        indoParaEsquerda = !indoParaEsquerda;
        sprite.flipX = !indoParaEsquerda;
    }
}
