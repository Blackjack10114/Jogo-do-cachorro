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

    public int quantidadeDePulos = 0;
    public bool grounded = false;
    private int groundContacts = 0;
    public bool ignorarpulo;

    private float coyoteTimer = 0f;
    private float jumpBufferTimer = 0f;

    public bool EstaNoChao => grounded;
    [SerializeField]private AudioSource audioPulo;
    private float tempoUltimoPulo = -999f;
    public float intervaloMinimoSomPulo = 0.1f; 
    public AudioClip[] sonsDePulo;
    public AudioMixerGroup sfxGroup;

    private Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMov = GetComponent<PlayerMov>();
        audioPulo = gameObject.AddComponent<AudioSource>();
        audioPulo.outputAudioMixerGroup = sfxGroup;
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
        bool velocidadeYZero = Mathf.Abs(rb.linearVelocity.y) < 0.01f;

        bool podePular = jumpBufferTimer > 0f &&
            (
                (coyoteTimer > 0f && velocidadeYZero) ||
                (playerMov.temPuloDuplo && quantidadeDePulos < 1)
            );


        if (quantidadeDePulos >= 1 && !velocidadeYZero)
        {
            podePular = false;
        }

        if (podePular || ((ignorarpulo && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)))))
        {
            float finalJumpForce = jumpForce;

            if (playerMov != null && playerMov.isTurboActive)
                finalJumpForce *= turboJumpMultiplier;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);
            
            if (Time.time - tempoUltimoPulo > intervaloMinimoSomPulo && sonsDePulo.Length > 0)
            {
                AudioClip somAleatorio = sonsDePulo[Random.Range(0, sonsDePulo.Length)];
                audioPulo.PlayOneShot(somAleatorio);
                tempoUltimoPulo = Time.time;
            }



            if (grounded)
            {
                coyoteTimer = 0f;
            }
            else if (playerMov.temPuloDuplo)
            {
                quantidadeDePulos++;
            }

            if (anim != null)
            {
                bool estaPulando = !grounded;
               // anim.SetBool("EstaPulando", estaPulando);
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
        collision.gameObject.CompareTag("Tatu") ||
        collision.gameObject.CompareTag("Untagged"))
        {
            groundContacts++;
            grounded = true;
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
            ignorarpulo = false;
        }
    }
    void FixedUpdate()
    {
        if (!grounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            grounded = true;
        }
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            quantidadeDePulos = 0;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovel"))
        {
            ignorarpulo = true;
            quantidadeDePulos = 0;
        }
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
