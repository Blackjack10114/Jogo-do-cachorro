using UnityEngine;

public class Candelabro : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float velocidadeQueda = 5f;

    private Rigidbody2D rb;
    private bool caiu = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    public void DetectouPlayer()
    {
        if (caiu) return;
        Cair();
    }

    private void Cair()
    {
        caiu = true;
        rb.gravityScale = velocidadeQueda;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!caiu) return;
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
