using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using static Mola;

public class Dano : MonoBehaviour
{
    public bool isInvincible = false;
    public float v, m;
    public float vidaMaxima = 100f;
    public float pv;

    private Rigidbody2D rb;

    public GameObject shield;
    private Caixa bool_script;

    private bool entregaFalhou = false;
    public AudioClip dano_som;
    AudioSource sound;
    public AudioMixerGroup sfxGroup;
    public bool Estasemcaixa;

    private Animator animDoug;

    public AudioSource audioDano;
    private float tempoUltimoDano = -999f;
    public float intervaloMinimoSomDano = 0.75f; 



    [SerializeField] private string cenaFalha;

    private static readonly string[] obstaculosQueCausamDano = {
        "Spike", "Buraco", "Tatu", "RaizRotatoria", "Passaro", "Meteorito", "Espada", "Fantasma", "Candelabro"
    };


    void Start()
    {
        audioDano = gameObject.AddComponent<AudioSource>();
        audioDano.outputAudioMixerGroup = sfxGroup;
        pv = vidaMaxima;
        bool_script = GetComponent<Caixa>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TomarDano(int dano, GameObject origem = null)
    {
        if (isInvincible) return;

        // Destroi bolha se tiver
        if (shield != null)
        {
            Destroy(shield);
            shield = null;
        }

        // Reduz vida
        if (!bool_script.caixaInstanciada)
        {
            pv -= dano;
            if (pv < 0f) pv = 0f;
        }

        // Knockback
        if (origem != null)
        {
            float direcao = (transform.position.x - origem.transform.position.x) >= 0 ? 1f : -1f;
            rb.linearVelocity = new Vector2(direcao * m * v, rb.linearVelocity.y);
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            if (Time.time - tempoUltimoDano > intervaloMinimoSomDano)
            {
                audioDano.PlayOneShot(dano_som);
                tempoUltimoDano = Time.time;
            }

        }

        // Sprite (caso espinho)
        if (origem != null)
        {
            Estasemcaixa = true;
            bool_script.CaixaPega = false;
        }

        GetComponent<PlayerMov>().enabled = false;
        StartCoroutine(DelayHabilitarMovim());

        StartCoroutine(DelayInvincibilityReset());
    }

    public void ActivateShield()
    {
        if (!isInvincible)
        {
            isInvincible = true;

            if (shield == null)
            {
                shield = Instantiate(Resources.Load<GameObject>("Bolha Protetora"), transform.position, Quaternion.identity);
                shield.transform.SetParent(transform);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        TratarColisao(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TratarColisao(other.gameObject);
    }

    private void TratarColisao(GameObject colisor)
    {
        if (!TagCausaDano(colisor.tag)) return;

        float direcao = (transform.position.x - colisor.transform.position.x) >= 0 ? 1f : -1f;

        if (!bool_script.CaixaPega)
        {
            // Sem caixa → só knockback (não perde vida da entrega)
            rb.linearVelocity = new Vector2(direcao * m * v, rb.linearVelocity.y);
            rb.AddForce(Vector2.up * 150, ForceMode2D.Impulse);
            StartCoroutine(LimitarKnockback(rb));

            if (Time.time - tempoUltimoDano > intervaloMinimoSomDano)
            {
                audioDano.PlayOneShot(dano_som);
                tempoUltimoDano = Time.time;
            }

            return; // sai da função, não chama TomarDano
        }

        

        if (isInvincible)
        {
            if (shield != null)
            {
                Destroy(shield);
                shield = null;
            }
            StartCoroutine(DelayInvincibilityReset());
            rb.linearVelocity = new Vector2(direcao * m * v, rb.linearVelocity.y);
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            return;
        }

        // Aplica dano padrão
        TomarDano(10, colisor);
    }

    private bool TagCausaDano(string tag)
    {
        foreach (string t in obstaculosQueCausamDano)
        {
            if (tag == t) return true;
        }
        return false;
    }

    private IEnumerator DelayInvincibilityReset()
    {
        yield return new WaitForSeconds(0.1f);
        isInvincible = false;
    }

    private IEnumerator DelayHabilitarMovim()
    {
        yield return new WaitForSeconds(0.3f);
        GetComponent<PlayerMov>().enabled = true;
    }

    IEnumerator LimitarKnockback(Rigidbody2D alvo)
    {
        yield return new WaitForFixedUpdate();
        if (alvo.linearVelocity.y > 25f)
        {
            alvo.linearVelocity = new Vector2(alvo.linearVelocity.x, 25f);
        }
    }

    void Update()
    {
        if (!entregaFalhou && pv <= 0f)
        {
            entregaFalhou = true;

            // Garante que falha sempre é "F" e 0 estrelas
            PlayerPrefs.SetString("ClassificacaoLetra", "F");
            PlayerPrefs.SetInt("PontuacaoNumerica", 0);
            PlayerPrefs.SetFloat("PontuacaoFinal", 0f); // estrelas
            PlayerPrefs.SetInt("BonusVida", 0);
            PlayerPrefs.SetInt("BonusTempo", 0);
            PlayerPrefs.SetInt("BonusTotal", 0);
            PlayerPrefs.SetInt("PontosOssos", 0);
            PlayerPrefs.SetInt("OssosColetados", 0);
            PlayerPrefs.SetFloat("TempoFinal", 0f);
            PlayerPrefs.SetFloat("VidaFinal", 0f);
            PlayerPrefs.SetInt("EntregaFalhou", 1);
            PlayerPrefs.Save();

            Time.timeScale = 1f;
            ProximaFaseController.RegistrarCenaAtual(SceneManager.GetActiveScene().name);
            SceneManager.LoadScene(cenaFalha);
        }

    }
}
