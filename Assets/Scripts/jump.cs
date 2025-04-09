using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] public float jumpForce;
    [SerializeField] private float turboJumpMultiplier;
    private Rigidbody2D rb;
    private bool Grounded;

    private PlayerMov playerMov;
    private int groundContacts = 0; // Contador de colisões com o chão

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
            groundContacts++; // Incrementa a contagem de colisões com o chão
            Grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundContacts--; // Decrementa ao sair do chão
            if (groundContacts <= 0)
            {
                Grounded = false; // Só desativa se não houver mais contatos com o chão
            }
        }
    }
}