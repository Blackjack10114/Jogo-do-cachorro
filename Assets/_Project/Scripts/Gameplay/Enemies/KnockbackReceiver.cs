using UnityEngine;

public class KnockbackReceiver : MonoBehaviour
{
    private Rigidbody2D rb;
    private float ultimoKnockback = -999f;
    public float intervaloKnockback = 0.2f;
    public float knockbackMaxY = 25f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public bool PodeReceberKnockback()
    {
        return Time.time - ultimoKnockback >= intervaloKnockback;
    }

    public void AplicarKnockback(Vector2 direcao, float forca)
    {
        if (!PodeReceberKnockback()) return;

        ultimoKnockback = Time.time;

        rb.AddForce(direcao.normalized * forca, ForceMode2D.Impulse);

        // Clamp na velocidade para evitar voo
        if (rb.linearVelocity.y > knockbackMaxY)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, knockbackMaxY);
    }
}
