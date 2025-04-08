using UnityEngine;
using System.Collections;

public class Dano : MonoBehaviour
{
    public bool isInvincible = false;
    public bool contato;
    public float v, m;
    public float pv = 30f;

    private Rigidbody2D rb;
    private float time = 0f;

    public GameObject shield;
    public GameObject Sprite_Dog_Caixa_Normal_0;

    public Sprite Sprite_Dog_Caixa_Normal;
    public Sprite Sprite_Dog_Sem_Caixa_0;

    private StunControllerComVida stun;

    // 🆕 Variáveis para queda
    private float velocidadeVerticalAnterior = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stun = GetComponent<StunControllerComVida>();
    }

    public IEnumerator ActivateShield(float duration)
    {
        if (!isInvincible)
        {
            isInvincible = true;

            if (shield == null)
            {
                shield = Instantiate(Resources.Load<GameObject>("Bolha Protetora"), transform.position, Quaternion.identity);
                shield.transform.SetParent(transform);
            }

            yield return new WaitForSeconds(duration);

            if (shield != null)
            {
                Destroy(shield);
                shield = null;
            }

            isInvincible = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Colidiu com: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Colidiu com chão. Verificando dano por queda...");
            AplicarDanoPorImpactoVertical(velocidadeVerticalAnterior);
        }

        TratarColisao(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TratarColisao(other.gameObject);
    }

    private void TratarColisao(GameObject colisor)
    {
        if (colisor.CompareTag("Spike") || colisor.CompareTag("Buraco"))
        {
            if (isInvincible)
            {
                if (shield != null)
                {
                    Destroy(shield);
                    shield = null;
                }

                StartCoroutine(DelayInvincibilityReset());
                return;
            }

            GetComponent<PlayerMov>().enabled = false;
            rb.linearVelocity = new Vector2(-m * v, rb.linearVelocity.y);
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

            GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa_0;

            if (stun != null)
            {
                stun.TomarDano(10f);
            }

            if (!TemCaixa())
            {
                pv -= 10f;
                if (pv < 0f) pv = 0f;
                Object.FindFirstObjectByType<SistemaPontuacao>()?.AdicionarColisao();
            }
        }
    }

    private IEnumerator DelayInvincibilityReset()
    {
        yield return new WaitForSeconds(0.1f);
        isInvincible = false;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            GetComponent<PlayerMov>().enabled = true;
            time = 0f;
        }

        if (rb != null)
            velocidadeVerticalAnterior = rb.linearVelocity.y;
    }

    private void AplicarDanoPorImpactoVertical(float velocidade)
    {
        Debug.Log("Velocidade vertical: " + velocidade);

        float velocidadeAbs = Mathf.Abs(velocidade);

        if (velocidade > -3f) // ajustável conforme quiser
        {
            Debug.Log("Impacto muito fraco. Sem dano.");
            return;
        }

        if (!TemCaixa())
        {
            Debug.Log("Sem caixa - Sem dano.");
            return;
        }

        float dano = 0f;

        if (velocidadeAbs >= 30f)
        {
            dano = 15f;
            Debug.Log("⚠️ GRANDE IMPACTO - Dano 15%");
        }
        else if (velocidadeAbs >= 20f)
        {
            dano = 10f;
            Debug.Log("⚠️ MÉDIO IMPACTO - Dano 10%");
        }
        else if (velocidadeAbs >= 10f)
        {
            dano = 5f;
            Debug.Log("⚠️ PEQUENO IMPACTO - Dano 5%");
        }

        if (dano > 0f)
        {
            pv -= dano;
            if (pv < 0f) pv = 0f;
            Object.FindFirstObjectByType<SistemaPontuacao>()?.AdicionarColisao();
        }
    }

    private bool TemCaixa()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return false;

        return sr.sprite == Sprite_Dog_Caixa_Normal;
    }
}
