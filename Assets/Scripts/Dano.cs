using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Dano : MonoBehaviour
{
    public bool isInvincible = false;
    public float v, m;
    public float pv = 30f;

    private Rigidbody2D rb;
    private float time = 0f;

    public GameObject shield;
    public GameObject Sprite_Dog_Caixa_Normal_0;
    private Caixa bool_script;

    public Sprite Sprite_Dog_Caixa_Normal;
    public Sprite Sprite_Dog_Sem_Caixa;

    private bool entregaFalhou = false;

    private static readonly string[] obstaculosQueCausamDano = {
        "Spike", "Buraco", "Tatu", "RaizRotatoria", "Passaro", "Meteorito"
    };

    private static readonly string[] obstaculosQueCaemCaixa = {
        "Spike", "Tatu", "RaizRotatoria", "Passaro", "Meteorito"
    };


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<Caixa>();
    }

    public void TomarDano(int dano, GameObject origem = null)
    {
        if (isInvincible) return;

        isInvincible = true;

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
        }

        // Sprite (caso espinho)
        if (origem != null && origem.CompareTag("Spike"))
        {
            GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;
        }

        GetComponent<PlayerMov>().enabled = false;

        StartCoroutine(DelayInvincibilityReset());
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
        // Se for um meteorito, não tratamos aqui - deixe o script do meteorito lidar
        if (colisor.CompareTag("Meteorito")) return;

        if (!TagCausaDano(colisor.tag)) return;

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

        // Aplica dano padrão
        TomarDano(10, colisor);
    }

    private bool TagCaiCaixa(string tagcaixa)
    {
        foreach (string t in obstaculosQueCaemCaixa)
        {
            if (tagcaixa == t) return true;
        }
        return false;
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

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            GetComponent<PlayerMov>().enabled = true;
            time = 0f;
        }

        if (!entregaFalhou && pv <= 0f)
        {
            entregaFalhou = true;
            PlayerPrefs.SetInt("EntregaFalhou", 1);
            PlayerPrefs.Save();
            Time.timeScale = 1f;
            SceneManager.LoadScene("CenaFimDeFase");
        }
    }
}
