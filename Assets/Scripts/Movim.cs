﻿using System.Collections;
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
    public float TempoPulo;

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
    private bool somCorridaTocando = false;

    [SerializeField] private float Velocidadeanimacao;

    Animator animDoug;

    void Start()
    {
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
        if (isRunning)
        {
            animDoug.speed = Velocidadeanimacao;
        }
        if (!isRunning)
        {
            animDoug.speed = 1f;
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
        bool estaAndando = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ||
                   Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

        bool podeTocarSomCorrida = pulo != null && pulo.EstaNoChao && isRunning && estaAndando && stamina > 0;

        if (podeTocarSomCorrida && !somCorridaTocando)
        {
            sound.clip = Correr_som;
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

        if (temPuloDuplo == true)
        {
            TempoPulo -= Time.deltaTime;
        }

        bool andando = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) ||
                   Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);

        animDoug.SetBool("EstaAndando", andando);

        // Verifica se está no chão
        bool grounded = pulo != null && pulo.EstaNoChao;
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
            {
                stamina -= 0.2f * staminaConsumptionMultiplier * turboStaminaReduction;
            }
        }

        if (pulo != null && pulo.EstaNoChao)
        {
            wasRunningBeforeJump = isRunning;
        }

        float airControl = wasRunningBeforeJump ? sprintSpeedMultiplier * turboMultiplier : 1f;

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
            rb.linearVelocity = new Vector2(direction * move * speed * airControl+ velocidadePlataforma, rb.linearVelocity.y);
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
