using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMov : MonoBehaviour
{
    [HideInInspector] public bool temPuloDuplo = false;

    public bool isGourmetActive = false;
    public bool isTurboActive = false;

    public float speed = 5f;
    public float move = 1f;
    public float stamina = 100f;
    public float duracaoPuloDuploAtual = 0f;

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
    private float velocidadePlataforma = 0f;
    [HideInInspector] public bool IndoEsquerda;
    [HideInInspector] public bool IndoDireita;
    AudioSource sound;
    public AudioClip Correr_som;
    public AudioMixerGroup sfxGroup;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pulo = GetComponent<Jump>();
        sound = gameObject.GetComponent<AudioSource>();
        sound.outputAudioMixerGroup = sfxGroup;
    }

    public void AplicarVelocidadePlataforma(float vel)
    {
        velocidadePlataforma = vel;
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
            isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && (stamina > 0 || isGourmetActive);
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
            IndoEsquerda = false;
            IndoDireita = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            MovePlayer(-1);
            IndoDireita = false;
            IndoEsquerda = true;
        }
            bool estaParado =
            !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D) &&
            !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A);

        if (estaParado && plataformaAtual != null && pulo != null && pulo.EstaNoChao)
        {
            rb.linearVelocity = new Vector2(velocidadePlataforma, rb.linearVelocity.y);
        }
        if (stamina <= 0)
        {
            stamina = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            sound.clip = Correr_som;
            sound.Play();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            sound.Stop();
        }
        duracaoPuloDuploAtual -= Time.deltaTime;
        if (duracaoPuloDuploAtual <= 0)
        {
            duracaoPuloDuploAtual = 0;
        }
    }

    private void MovePlayer(int direction)
    {
        GetComponent<SpriteRenderer>().flipX = (direction == -1);

        float finalSpeed = speed;

        if (pulo != null && pulo.EstaNoChao && isRunning && stamina > 0)
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
            float velX = direction * move * finalSpeed;

            if (plataformaAtual != null)
            {
                velX += velocidadePlataforma;
            }

            rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);

            if (time >= 0.8f)
            {
                rb.linearVelocity = new Vector2(direction * move * finalSpeed + (direction * 5), rb.linearVelocity.y);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(direction * move * speed * airControl * turboAirControl + velocidadePlataforma, rb.linearVelocity.y);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            time = 0;
        }

        velocidadePlataforma = 0f;
    }

    public void HabilitarMovimento(bool estado)
    {
        podeMover = estado;
    }

    public IEnumerator AtivarPuloDuploTemporario(float duracao)
    {
        temPuloDuplo = true;
        duracaoPuloDuploAtual = duracao;
        yield return new WaitForSeconds(duracao);
        temPuloDuplo = false;
        duracaoPuloDuploAtual = 0f;
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
