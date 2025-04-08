using System.Collections;
using UnityEngine;

public class Movim_tatu_Esquerda : MonoBehaviour
{
    public float speed, move;
    private Rigidbody2D rb;
    public bool Esquerda;
    public bool Direita;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        Esquerda = false;
        Direita = true;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
        if (collision.gameObject.tag == ("Wall"))
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
            Esquerda = false;
            Direita = true;
        }
        if (collision.gameObject.tag == ("Walld"))
        {
            rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
            Esquerda = true;
            Direita = false;
        }
        if (collision.gameObject.tag == ("Spike") && Esquerda)
        {
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
            Esquerda = false;
            Direita = true;
        }
        if (collision.gameObject.tag == ("Spike") && Direita)
        {
            StartCoroutine(DelayLadoTatu());
        }
    }
    private IEnumerator DelayLadoTatu()
    {
        yield return new WaitForSeconds(1f);
        Esquerda = true;
        Direita = false;
        rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
    }
}
