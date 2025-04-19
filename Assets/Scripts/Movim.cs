using System.Collections;
using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public bool isGourmetActive = false;
    public bool isTurboActive = false;

    public float speed = 5f;
    public float move = 1f;
    public float stamina = 100f;

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
    private Jump pulo;

    public bool podeMover = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pulo = GetComponent<Jump>(); // ← acessa o script Jump
    }

    void Update()
    {
        if (!podeMover)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (pulo != null && pulo.EstaNoChao)
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
    }

    private void MovePlayer(int direction)
    {
        GetComponent<SpriteRenderer>().flipX = (direction == -1);

        float finalSpeed = speed;

        if (pulo != null && pulo.EstaNoChao && isRunning)
        {
            finalSpeed *= sprintSpeedMultiplier * turboMultiplier;

            if (!isGourmetActive)
            {
                stamina -= 0.2f * staminaConsumptionMultiplier * turboStaminaReduction;
            }
        }

        if (pulo != null && pulo.EstaNoChao)
        {
            wasRunningBeforeJump = isRunning;
        }

        float airControl = wasRunningBeforeJump ? sprintSpeedMultiplier * turboMultiplier : 1f;
        float turboAirControl = isTurboActive ? 0.6f : 1f;

        if (pulo != null && pulo.EstaNoChao)
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

    public void HabilitarMovimento(bool estado)
    {
        podeMover = estado;
    }

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
        if (collision.gameObject.CompareTag("PlataformaMovel") || collision.gameObject.CompareTag("PlataformaQuebradica"))
        {
            plataformaAtual = collision.gameObject.GetComponent<PlataformaMovel>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovel"))
        {
            plataformaAtual = null;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlataformaMovel"))
        {
            plataformaAtual = collision.gameObject.GetComponent<PlataformaMovel>();
        }
    }
}
