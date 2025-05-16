using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Audio;

public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float turboJumpMultiplier = 1.2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private Rigidbody2D rb;
    private PlayerMov playerMov;

    private bool pulouDuplo = false;
    public bool grounded = false;
    private int groundContacts = 0;

    private float coyoteTimer = 0f;
    private float jumpBufferTimer = 0f;

    public bool EstaNoChao => grounded;
    AudioSource sound;
    public AudioClip som_pulo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMov = GetComponent<PlayerMov>();
        sound = gameObject.GetComponent<AudioSource>();
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

        bool podePular = jumpBufferTimer > 0f &&
                         (coyoteTimer > 0f || (playerMov.temPuloDuplo && !pulouDuplo));

        if (podePular)
        {
            float finalJumpForce = jumpForce;

            if (playerMov != null && playerMov.isTurboActive)
                finalJumpForce *= turboJumpMultiplier;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);
            sound.clip = som_pulo;
            sound.Play();

            if (grounded)
            {
                coyoteTimer = 0f;
            }
            else if (playerMov.temPuloDuplo)
            {
                pulouDuplo = true;
            }

            jumpBufferTimer = 0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
        collision.gameObject.CompareTag("PlataformaMovel") ||
        collision.gameObject.CompareTag("PlataformaQuebradica") ||
        collision.gameObject.CompareTag("Spike") ||
        collision.gameObject.CompareTag("RaizRotatoria") ||
        collision.gameObject.CompareTag("Meteorito") ||
        collision.gameObject.CompareTag("Passaro") ||
        collision.gameObject.CompareTag("Tatu")   ||
        collision.gameObject.CompareTag("Untagged"))
        {
            groundContacts++;
            grounded = true;

            // ✅ Resetar pulo duplo apenas se ele estiver ativo
            if (playerMov.temPuloDuplo)
                pulouDuplo = false;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") ||
            collision.gameObject.CompareTag("PlataformaMovel") ||
            collision.gameObject.CompareTag("PlataformaQuebradica") ||
            collision.gameObject.CompareTag("Spike") ||
            collision.gameObject.CompareTag("RaizRotatoria") ||
            collision.gameObject.CompareTag("Meteorito") ||
            collision.gameObject.CompareTag("Passaro") ||
            collision.gameObject.CompareTag("Tatu") ||
            collision.gameObject.CompareTag("Untagged"))
        {
            groundContacts = Mathf.Max(0, groundContacts - 1);
            if (groundContacts == 0)
                grounded = false;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        bool isTouchingGround = false;

        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            if (hitPos.normal.y > 0.5f)
            {
                isTouchingGround = true;
                break;
            }
        }

        grounded = isTouchingGround;
    }

}
