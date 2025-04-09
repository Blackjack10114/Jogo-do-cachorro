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
    private Caixa bool_script;

    public Sprite Sprite_Dog_Caixa_Normal;
    public Sprite Sprite_Dog_Sem_Caixa;

    private StunControllerComVida stun;
    private SpriteRenderer sr;

    [SerializeField] private float alturaMinimaParaDano = 10f;
    private float alturaInicialDaQueda = 0f;
    private bool estaCaindo = false;
    private float tempoNoAr = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<Caixa>();
        stun = GetComponent<StunControllerComVida>();
        sr = GetComponent<SpriteRenderer>();
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
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("PlataformaMovel") || collision.gameObject.CompareTag("PlataformaQuebradica")) && estaCaindo)
        {
            float alturaFinal = transform.position.y;
            float diferencaAltura = alturaInicialDaQueda - alturaFinal;
            estaCaindo = false;

            Debug.Log($"Queda detectada: alturaInicial = {alturaInicialDaQueda:F2}, alturaFinal = {alturaFinal:F2}, diferença = {diferencaAltura:F2}");

            if (!bool_script.caixaInstanciada)
            {
                if (diferencaAltura > alturaMinimaParaDano)
                {
                    if (isInvincible)
                    {
                        Debug.Log("🛡️ Queda causaria dano, mas o jogador está com bolha. Sem dano!");
                        return;
                    }

                    pv -= 10f;
                    if (pv < 0f) pv = 0f;

                    StartCoroutine(FeedbackDano());
                    Debug.Log($"➡️ Tomou dano por queda! Vida atual: {pv}");
                    Object.FindFirstObjectByType<SistemaPontuacao>()?.AdicionarColisao();
                }
                else
                {
                    Debug.Log("✅ Queda sem dano: altura abaixo do limite.");
                }
            }
            else
            {
                Debug.Log("📦 Estava com a caixa — queda ignorada.");
            }
        }

        TratarColisao(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TratarColisao(other.gameObject);
    }

    private void TratarColisao(GameObject colisor)
    {
        if (colisor.CompareTag("Spike") || colisor.CompareTag("Buraco") || colisor.CompareTag("Tatu"))
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

            float direcao = (transform.position.x - colisor.transform.position.x) >= 0 ? 1f : -1f;
            rb.linearVelocity = new Vector2(direcao * m * v, rb.linearVelocity.y);
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

            if (colisor.CompareTag("Spike"))
            {
                sr.sprite = Sprite_Dog_Sem_Caixa;
            }

            if (stun != null)
            {
                stun.TomarDano(10f);
            }

            if (!bool_script.caixaInstanciada)
            {
                pv -= 10f;
                if (pv < 0f) pv = 0f;

                StartCoroutine(FeedbackDano());
                Object.FindFirstObjectByType<SistemaPontuacao>()?.AdicionarColisao();
            }
        }
    }

    private IEnumerator FeedbackDano()
    {
        isInvincible = true;

        for (int i = 0; i < 4; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        isInvincible = false;
    }

    private IEnumerator DelayInvincibilityReset()
    {
        yield return new WaitForSeconds(0.1f);
        isInvincible = false;
    }

    void Update()
    {
        bool estaNoChao = Physics2D.Raycast(transform.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));

        if (!estaNoChao)
        {
            tempoNoAr += Time.deltaTime;

            if (rb.linearVelocity.y < -0.1f && !estaCaindo && tempoNoAr > 0.1f)
            {
                estaCaindo = true;
                alturaInicialDaQueda = transform.position.y;
                Debug.Log($"📏 Início da queda registrado: {alturaInicialDaQueda:F2}");
            }
        }
        else
        {
            tempoNoAr = 0f;
        }

        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            GetComponent<PlayerMov>().enabled = true;
            time = 0f;
        }
    }
}
