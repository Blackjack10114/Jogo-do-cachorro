using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float turboJumpMultiplier = 1.2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private Rigidbody2D rb;
    private PlayerMov playerMov;

    private bool grounded = false;
    private int groundContacts = 0;

    private float coyoteTimer = 0f;
    private float jumpBufferTimer = 0f;

    public bool EstaNoChao => grounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMov = GetComponent<PlayerMov>();
    }

    void Update()
    {
        // Buffer de pulo
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
        {
            jumpBufferTimer = jumpBufferTime;
        }
        else
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // Atualiza coyote time
        if (grounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        // Executa pulo se dentro do tempo de toler�ncia
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            float finalJumpForce = jumpForce;
            if (playerMov != null && playerMov.isTurboActive)
            {
                finalJumpForce *= turboJumpMultiplier;
            }

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);

            grounded = false;
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("PlataformaMovel") ||
            collision.gameObject.CompareTag("PlataformaQuebradica"))
        {
            groundContacts++;
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("PlataformaMovel") ||
            collision.gameObject.CompareTag("PlataformaQuebradica"))
        {
            groundContacts = Mathf.Max(0, groundContacts - 1);
            if (groundContacts == 0)
            {
                grounded = false;
            }
        }
    }
}
