using UnityEngine;

public class Pulo_Caixa : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] public float jumpforce;
    public float speed, move;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
        }
    }
}
