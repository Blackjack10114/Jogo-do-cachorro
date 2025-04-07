using UnityEngine;

public class Movim_tatu : MonoBehaviour
{
    public float speed, move;
    private Rigidbody2D rb;
    public GameObject Tatu = null;
    public GameObject Caixa_Separada_0 = null;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
        if (collision.gameObject.tag == ("Wall"))
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        }
        if (collision.gameObject.tag == ("Walld"))
        {
            rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
        }
    }
    void Update()
    {
        Physics2D.IgnoreCollision(Tatu.GetComponent<Collider2D>(), Caixa_Separada_0.GetComponent<Collider2D>());
    }
}
