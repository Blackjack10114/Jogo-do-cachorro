using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] public float jumpForce;
    [SerializeField] private float turboJumpMultiplier;
    private Rigidbody2D rb;
    private bool Grounded;

    private PlayerMov playerMov;
    private int groundContacts = 0; // Contador de colis�es com o ch�o

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMov = GetComponent<PlayerMov>();
    }

    void Update()
    {
        if (Grounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            float finalJumpForce = jumpForce;

            if (playerMov != null && playerMov.isTurboActive)
            {
                finalJumpForce *= turboJumpMultiplier;
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            rb.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);
            Grounded = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts++; // Incrementa a contagem de colis�es com o ch�o
            Grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts--; // Decrementa ao sair do ch�o
            if (groundContacts <= 0)
            {
                Grounded = false; // S� desativa se n�o houver mais contatos com o ch�o
            }
        }
    }
}