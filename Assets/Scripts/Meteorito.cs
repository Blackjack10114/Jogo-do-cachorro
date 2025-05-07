using UnityEngine;

public class Meteorito : MonoBehaviour
{
    public float delayQueda = 1f;
    public float forcaQueda = 15f;
    public GameObject efeitoExplosao;

    private Rigidbody2D rb;
    private bool ativado = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // inicialmente sem gravidade, sem movimento
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    public void AtivarQueda()
    {
        if (!ativado)
        {
            ativado = true;
            Invoke(nameof(IniciarQueda), delayQueda);
        }
    }

    void IniciarQueda()
    {
        // Agora sim aplica gravidade e deixa o Unity cuidar da queda
        rb.gravityScale = 1f;
        rb.linearVelocity = Vector2.down * forcaQueda;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Dano no jogador
        if (other.CompareTag("Player"))
        {
            Dano dano = other.GetComponent<Dano>();
            if (dano != null)
            {
                dano.TomarDano(10, gameObject);
            }
        }

        // Se colidir com chão ou plataformas, destruir também
        if (other.CompareTag("Ground") ||
            other.CompareTag("PlataformaMovel") ||
            other.CompareTag("PlataformaQuebradica"))
        {
            if (efeitoExplosao != null)
            {
                Instantiate(efeitoExplosao, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }

        // Se colidiu com qualquer coisa relevante (player ou chão), destrói
        if (other.CompareTag("Player") ||
            other.CompareTag("Ground") ||
            other.CompareTag("PlataformaMovel") ||
            other.CompareTag("PlataformaQuebradica"))
        {
            if (efeitoExplosao != null)
            {
                Instantiate(efeitoExplosao, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

}
