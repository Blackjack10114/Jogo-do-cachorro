using UnityEngine;

public class Movim_tatu : MonoBehaviour
{
    public float speed, move;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
    }

    // Update is called once per frame
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
}
