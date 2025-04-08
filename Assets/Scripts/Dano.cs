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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<Caixa>();
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

            // Dano físico / feedback
            GetComponent<PlayerMov>().enabled = false;
            rb.linearVelocity = new Vector2(-m * v, rb.linearVelocity.y); // Knockback
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse); // Pulo

            // Troca de sprite
            GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;

            // Sempre aplica dano para o sistema de stun
            if (stun != null)
            {
                stun.TomarDano(10f);
            }

            // Só perde PV se estiver sem a caixa
            if (!bool_script.caixaInstanciada)
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
    }
}
