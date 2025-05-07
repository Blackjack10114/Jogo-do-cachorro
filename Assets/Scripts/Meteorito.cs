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
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.down * forcaQueda;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Meteorito"))
        {
            ExplodirEDestruir();
        }
    }

    void ExplodirEDestruir()
    {
        if (efeitoExplosao != null)
        {
            Instantiate(efeitoExplosao, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
