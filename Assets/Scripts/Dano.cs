using UnityEngine;
using System.Collections;

public class Dano : MonoBehaviour
{
    public bool isInvincible = false;
    public bool contato;
    public float v, m;
    public float pv = 30f; // Valor inicial de vida, pode ser ajustado no Inspetor

    private Rigidbody2D rb;
    private float time = 0f;

    public GameObject shield;
    public GameObject Sprite_Dog_Caixa_Normal_0;
    private Caixa bool_script;

    public Sprite Sprite_Dog_Caixa_Normal;
    public Sprite Sprite_Dog_Sem_Caixa;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<Caixa>();
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            if (isInvincible && shield != null)
            {
                Destroy(shield);
                shield = null;
                StartCoroutine(DelayInvincibilityReset());
                return;
            }

            GetComponent<PlayerMov>().enabled = false;
            rb.linearVelocity = new Vector2(-m * v, rb.linearVelocity.y);
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

            GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;

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
