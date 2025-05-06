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
        rb.gravityScale = 0f; // gravidade desligada inicialmente
        rb.linearVelocity = Vector2.zero; // previne queda prematura
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
        rb.gravityScale = 0f; // evita gravidade acumulada
        rb.linearVelocity = Vector2.down * forcaQueda; // queda direta com velocidade definida
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colidiu com: " + collision.gameObject.name); // ADICIONE ESTA LINHA

        string tag = collision.gameObject.tag;

        if (tag == "Player" || tag == "Ground" || tag == "PlataformaMovel" || tag == "PlataformaQuebradica")
        {
            if (efeitoExplosao != null)
            {
                Instantiate(efeitoExplosao, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); 
        }
    }

}
