using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class PlayerMov : MonoBehaviour
{

    private InputController inputActions;

    [HideInInspector] public bool temPuloDuplo = false;

    public bool isGourmetActive = false;
    public bool isTurboActive = false;

    public float speed = 5f;
    public float move = 1f;
    public float stamina = 100f;
    public float TempoPulo;

    public float sprintSpeedMultiplier = 2f;
    public float staminaConsumptionMultiplier = 1.0f; //multiplica o gasto de stamina quando corre

    public float turboTimer = 0f;
    public float gourmetTimer = 0f;

    public float turboMultiplier; //multiplica o gasto de stamina quando corre
    public float turboStaminaReduction = 1f; //diminui o gasto de stamina um pouco com o turbo, por enquanto n interfere, mas manteve caso precise equilibrar no futuro

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
    private bool somCorridaTocando = false;

    [SerializeField] private float Velocidadeanimacao;

    Animator animDoug;

    private Comeco_Fase comecoFase; // Variavel que verifica se começou a fase para liberar o movimento


    void Awake()
    {
        inputActions = new InputController();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        comecoFase = FindFirstObjectByType<Comeco_Fase>();// Procura um objeto da cena com script Comeco_Fase
        Velocidadeanimacao = 2.5f;
        animDoug = GetComponent<Animator>();
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
        // Verica se o Script comeco fase está ativo, se tiver ele bloqueia o mov, se n ele já começa ativo
        if (comecoFase != null && !Comeco_Fase.FaseComecou) 
        {
            rb.linearVelocity = Vector2.zero;
            animDoug.SetBool("EstaAndando", false);
            animDoug.SetBool("Grounded", pulo != null && pulo.EstaNoChao);

            PararSomCorrida(); // Evita que o som continue antes da fase começar

            return; // Interrompe toda a lógica até a fase começar
        }

        if (!podeMover)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 moveInput = inputActions.Player.Mov.ReadValue<Vector2>();
        bool estaAndando = Mathf.Abs(moveInput.x) > 0.01f;
        bool grounded = pulo != null && pulo.EstaNoChao;

        isRunning = inputActions.Player.Run.IsPressed() && (stamina > 0 || isGourmetActive);

        if (!isRunning && stamina < 100)
            stamina += Time.deltaTime * 20;

        animDoug.speed = isRunning ? Velocidadeanimacao : 1f;

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
                isGourmetActive = false;
        }

        if (estaAndando)
        {
            int direction = moveInput.x > 0 ? 1 : -1;
            MovePlayer(direction);
            IndoDireita = direction == 1;
            IndoEsquerda = direction == -1;
        }
        else if (plataformaAtual != null && grounded)
        {
            rb.linearVelocity = new Vector2(velocidadePlataforma, rb.linearVelocity.y);
        }

        if (stamina <= 0)
            stamina = 0;

        bool podeTocarSomCorrida = grounded && isRunning && estaAndando && stamina > 0;

        if (podeTocarSomCorrida && !somCorridaTocando)
        {
            sound.clip = Correr_som;
            sound.volume = 1.4f;
            sound.loop = true;
            sound.Play();
            somCorridaTocando = true;
        }
        else if (!podeTocarSomCorrida && somCorridaTocando)
        {
            sound.Stop();
            sound.loop = false;
            somCorridaTocando = false;
        }

        if (temPuloDuplo)
            TempoPulo -= Time.deltaTime;

        animDoug.SetBool("EstaAndando", estaAndando);
        animDoug.SetBool("Grounded", grounded);
    }


    private void MovePlayer(int direction)
    {
        GetComponent<SpriteRenderer>().flipX = (direction == -1);

        float finalSpeed = speed;

        if (pulo != null && pulo.EstaNoChao && isRunning && stamina > 0)
        {
            finalSpeed *= sprintSpeedMultiplier * turboMultiplier;

            if (!isGourmetActive)
                stamina -= 10f * staminaConsumptionMultiplier * turboStaminaReduction * Time.deltaTime;
        }

        if (pulo != null && pulo.EstaNoChao)
            wasRunningBeforeJump = isRunning;

        float airControl = wasRunningBeforeJump ? sprintSpeedMultiplier * turboMultiplier : 1f;

        if (pulo != null && pulo.EstaNoChao)
        {
            time += Time.deltaTime;
            float velX = direction * move * finalSpeed;

            if (plataformaAtual != null)
                velX += velocidadePlataforma;

            rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);

            if (time >= 0.8f)
            {
                rb.linearVelocity = new Vector2(direction * move * finalSpeed + (direction * 5), rb.linearVelocity.y);
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(direction * move * speed * airControl + velocidadePlataforma, rb.linearVelocity.y);
        }

        // reset do time se parar
        if (Mathf.Abs(inputActions.Player.Mov.ReadValue<Vector2>().x) < 0.01f)
            time = 0;

        velocidadePlataforma = 0f;
    }

    public void HabilitarMovimento(bool estado)
    {
        podeMover = estado;
    }

    public IEnumerator AtivarPuloDuploTemporario(float duracao)
    {
        temPuloDuplo = true;
        TempoPulo = duracao;
        yield return new WaitForSeconds(duracao);
        temPuloDuplo = false;

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

    public void PararSomCorrida()
    {
        if (somCorridaTocando && sound != null)
        {
            sound.Stop();
            sound.loop = false;
            somCorridaTocando = false;
        }
    }

}
