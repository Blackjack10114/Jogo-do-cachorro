using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    private InputController inputActions;

    [SerializeField] private float jumpForce = 315f;
    [SerializeField] private float turboJumpMultiplier = 1.2f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;
    [SerializeField] float jumpCutMultiplier = 0.5f;

    [Header("Gravidade Extra")]
    [SerializeField] private float gravidadeBase = 1f;
    [SerializeField] private float multiplicadorQueda = 2.5f;
    [SerializeField] private float multiplicadorQuedaLonga = 3.5f;
    [SerializeField] private float velocidadeMaximaQueda = -25f;


    private Rigidbody2D rb;
    private PlayerMov playerMov;

    [HideInInspector] public bool ignorarLimiteDeQueda = false;
    [HideInInspector] public bool ignorarJumpCut = false;

    private Animator anim;

    public int quantidadeDePulos = 0;
    public bool grounded = false;
    public bool encostoupassavel;
    private int groundContacts = 0;
    public bool ignorarpulo;
    public bool ignorarProximoPulo = false;
    public bool molado;

    private float coyoteTimer = 0f;
    private float jumpBufferTimer = -1f;  // agora inicia negativo

    [SerializeField] private AudioSource audioPulo;
    private float tempoUltimoPulo = -999f;
    public float intervaloMinimoSomPulo = 0.1f;
    public AudioClip[] sonsDePulo;
    public AudioMixerGroup sfxGroup;

    public bool EstaNoChao => grounded;

    private Collider2D col;

    [SerializeField] private PhysicsMaterial2D groundMaterial;
    [SerializeField] private PhysicsMaterial2D airMaterial;


    void Awake()
    {
        inputActions = new InputController();
        inputActions.Player.Jump.performed += OnJumpPerformed;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerMov = GetComponent<PlayerMov>();
        col = GetComponent<Collider2D>();

        if (!audioPulo)
        {
            audioPulo = gameObject.AddComponent<AudioSource>();
            audioPulo.outputAudioMixerGroup = sfxGroup;
        }
    }

    /// Callback chamado quando botão de pulo é pressionado
    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (ignorarProximoPulo)
        {
            ignorarProximoPulo = false;
            return; // ignora o pulo no controle para o pause
        }

        jumpBufferTimer = jumpBufferTime;
    }

    void Update()
    {
        if (Comeco_Fase.inputParaComecarFoiUsado)
        {
            jumpBufferTimer = -1f; // Limpa o buffer de pulo para garantir
            return; // Pula fora do Update e ignora o resto
        }


        if (FindFirstObjectByType<Comeco_Fase>() != null && !Comeco_Fase.FaseComecou)
        {
            jumpBufferTimer = -1f;
            return;
        }

        if (PauseMenu.JogoPausado)
        {
            jumpBufferTimer = -1f; // garante que não acumula clique no pause
            return;
        }

        // Atualiza timers
        jumpBufferTimer -= Time.deltaTime;

        if (grounded)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;

        bool velocidadeYZero = Mathf.Abs(rb.linearVelocity.y) < 0.01f;

        bool podePular =
            jumpBufferTimer > 0f &&
            (
                (coyoteTimer > 0f && velocidadeYZero) ||
                (playerMov.temPuloDuplo && quantidadeDePulos < 1)
            );

        if (quantidadeDePulos >= 1 && !velocidadeYZero)
            podePular = false;

        if (podePular || (ignorarpulo && jumpBufferTimer > 0))
        {
            ExecutarPulo();
            jumpBufferTimer = -1f;  // consome o buffer
        }

        if (!ignorarJumpCut && !inputActions.Player.Jump.IsPressed() && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
        }



        if (grounded)
            col.sharedMaterial = groundMaterial;
        else
            col.sharedMaterial = airMaterial;

        if (!grounded && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            grounded = true;

        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            quantidadeDePulos = 0;
    }

    private void ExecutarPulo()
    {
        float finalJumpForce = jumpForce;

        if (playerMov != null && playerMov.isTurboActive)
            finalJumpForce *= turboJumpMultiplier;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * finalJumpForce, ForceMode2D.Impulse);

        float horizontalBoost = rb.linearVelocity.x * 0.04f;
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x + horizontalBoost,
            rb.linearVelocity.y
        );


        if (Time.time - tempoUltimoPulo > intervaloMinimoSomPulo && sonsDePulo.Length > 0)
        {
            AudioClip somAleatorio = sonsDePulo[Random.Range(0, sonsDePulo.Length)];
            audioPulo.PlayOneShot(somAleatorio);
            tempoUltimoPulo = Time.time;
        }

        if (grounded)
            coyoteTimer = 0f;
        else if (playerMov.temPuloDuplo)
            quantidadeDePulos++;
    }

    private void LateUpdate()
    {
        if (Comeco_Fase.inputParaComecarFoiUsado)
        {
            Comeco_Fase.inputParaComecarFoiUsado = false;
        }
    }
    void FixedUpdate()
    {
        // Se estiver subindo, usa gravidade normal
        if (rb.linearVelocity.y > 0)
            return;

        // Se estiver caindo
        if (!ignorarLimiteDeQueda && rb.linearVelocity.y < 0)
        {
            float multiplicador = inputActions.Player.Jump.IsPressed()
                ? multiplicadorQueda
                : multiplicadorQuedaLonga;

            rb.AddForce(
                Vector2.up * Physics2D.gravity.y * (multiplicador - gravidadeBase),
                ForceMode2D.Force
            );

            // Limita velocidade máxima de queda
            if (!ignorarLimiteDeQueda && rb.linearVelocity.y < velocidadeMaximaQueda)
            {
                Debug.LogWarning("limitando");
                rb.linearVelocity = new Vector2(
                    rb.linearVelocity.x,
                    velocidadeMaximaQueda
                );
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (molado && collision.gameObject.TryGetComponent<PlatformEffector2D>(out var effector))
        {
            encostoupassavel = true;
            Debug.Log("Colidiu com PlatformEffector2D");
            return;
        }
        if (IsGroundTag(collision.gameObject.tag))
        {
            groundContacts++;
            grounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGroundTag(collision.gameObject.tag))
        {
            groundContacts = Mathf.Max(0, groundContacts - 1);
            if (groundContacts == 0)
                grounded = false;
            ignorarpulo = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovel"))
        {
            ignorarpulo = true;
            quantidadeDePulos = 0;
        }

        /*bool isTouchingGround = false;
        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            if (hitPos.normal.y > 0.5f)
            {
                isTouchingGround = true;
                break;
            }
        }

        grounded = isTouchingGround;
        */
    }

    private bool IsGroundTag(string tag)
    {
        return tag == "Ground" || tag == "PlataformaMovel" || tag == "PlataformaQuebradica" ||
               tag == "Spike" || tag == "RaizRotatoria" || tag == "Meteorito" ||
               tag == "Passaro" || tag == "Tatu" || tag == "Untagged";
    }

    public void PararSomPulo()
    {
        if (audioPulo != null && audioPulo.isPlaying)
        {
            audioPulo.Stop();
        }
    }

}
