using UnityEngine;

public class Movim_tatu : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb;
    private bool indoParaEsquerda = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            indoParaEsquerda = !indoParaEsquerda;
            GetComponent<SpriteRenderer>().flipX = !indoParaEsquerda;
        }
    }

}
