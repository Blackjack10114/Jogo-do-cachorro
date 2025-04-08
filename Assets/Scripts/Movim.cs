using System.Collections;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public bool isGourmetActive = false;
    public bool isTurboActive = false;
    private bool Grounded;

    public float speed = 5f;
    public float move = 1f;
    public float stamina = 100f;
    public float jumpForce = 100f;

    public float sprintSpeedMultiplier = 2f;
    public float staminaConsumptionMultiplier = 1.0f;

    public float turboTimer = 0f;
    public float gourmetTimer = 0f;

    [HideInInspector] public float turboMultiplier = 1f;
    public float turboStaminaReduction = 1f;

    private Rigidbody2D rb;
    private float time = 0;
    private bool isRunning = false;
    private bool wasRunningBeforeJump = false;

    private PlataformaMovel plataformaAtual = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Grounded)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift) && (stamina > 0 || isGourmetActive);
        }

        if (!isRunning && stamina < 100)
        {
            stamina += Time.deltaTime * 20;
        }

        if (isTurboActive)
        {
            turboTimer -= Time.deltaTime;
            if (turboTimer <= 0)
            {
                isTurboActive = false;
                turboMultiplier = 1f;
                turboStaminaReduction = 1f;
            }
        }

        if (isGourmetActive)
        {
            gourmetTimer -= Time.deltaTime;
            if (gourmetTimer <= 0)
            {
                isGourmetActive = false;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            MovePlayer(1);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            MovePlayer(-1);
        }

        if (Grounded && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            Jump();
        }
        // No Update:
        if (!podeMover) return; // bloqueia movimento se atordoado
    }

    private void MovePlayer(int direction)
    {
        GetComponent<SpriteRenderer>().flipX = (direction == -1);

        float finalSpeed = speed;

        if (Grounded && isRunning)
        {
            finalSpeed *= sprintSpeedMultiplier * turboMultiplier;

            if (!isGourmetActive)
            {
                stamina -= 0.2f * staminaConsumptionMultiplier * turboStaminaReduction;
            }
        }

        if (Grounded)
        {
            wasRunningBeforeJump = isRunning;
        }

        float airControl = wasRunningBeforeJump ? sprintSpeedMultiplier * turboMultiplier : 1f;
        float turboAirControl = isTurboActive ? 0.6f : 1f;

        if (Grounded)
        {
            time += Time.deltaTime;
            rb.linearVelocity = new Vector2(direction * move * finalSpeed, rb.linearVelocity.y);

            if (time >= 0.8f)
            {
                rb.linearVelocity = new Vector2(direction * move * finalSpeed + (direction * 5), rb.linearVelocity.y);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(direction * move * speed * airControl * turboAirControl, rb.linearVelocity.y);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            time = 0;
        }
    }

    private void Jump()
    {
        // Zera a velocidade vertical antes de pular
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

        // Aplica o pulo
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        Grounded = false;
        plataformaAtual = null;
    }

    public bool podeMover = true;

    public void HabilitarMovimento(bool estado)
    {
        podeMover = estado;
    }




    // Adicione este trecho para aplicar movimento da plataforma ao jogador
    void FixedUpdate()
    {
        if (plataformaAtual != null)
        {
            Vector3 movimentoPlataforma = plataformaAtual.GetComponent<Rigidbody2D>().linearVelocity * Time.fixedDeltaTime;
            transform.position += movimentoPlataforma;
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
            plataformaAtual = null;
        }
        else if (collision.gameObject.CompareTag("PlataformaMovel") || (collision.gameObject.CompareTag("PlataformaQuebradica")))
        {
            Grounded = true;
            plataformaAtual = collision.gameObject.GetComponent<PlataformaMovel>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("PlataformaMovel"))
        {
            Grounded = false;
            plataformaAtual = null;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
            plataformaAtual = null;
        }
        else if (collision.gameObject.CompareTag("PlataformaMovel"))
        {
            Grounded = true;
            plataformaAtual = collision.gameObject.GetComponent<PlataformaMovel>();
        }
    }
}
