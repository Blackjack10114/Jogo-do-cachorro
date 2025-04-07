using UnityEngine;
using System.Collections;

public class Dano : MonoBehaviour
{
    public bool isInvincible = false;
    public bool contato;
    public float v, m;
    private Rigidbody2D rb;
    private float time = 0;
    public GameObject shield; // Variável para armazenar a bolha
    public float pv;
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

            // Se já existir uma bolha ativa, não cria outra
            if (shield == null)
            {
                shield = Instantiate(Resources.Load<GameObject>("Bolha Protetora"), transform.position, Quaternion.identity);
                shield.transform.SetParent(transform); // Faz a bolha seguir o jogador
            }

            yield return new WaitForSeconds(duration);

            // Quando o tempo acabar, remove o escudo se ele ainda existir
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
        if (collision.gameObject.tag == "Spike")
        {
            if (isInvincible && shield != null)
            {
                Destroy(shield); // Remove a bolha imediatamente
                shield = null;
                StartCoroutine(DelayInvincibilityReset());

                return; // Sai da função e evita que o dano seja aplicado
            }
            GameObject varGameObject = GameObject.FindWithTag("Player");
            GetComponent<PlayerMov>().enabled = false;
            rb.linearVelocity = new Vector2(-m * v, rb.linearVelocity.y);
            rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
            time += Time.deltaTime;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;
            if (bool_script.caixaInstanciada == false)
            {
                pv = pv - 10;
                if (pv < 0)
                {
                    pv = 0;
                }
            }
        }
    }
    private IEnumerator DelayInvincibilityReset()
    {
        yield return new WaitForSeconds(0.1f);
        isInvincible = false;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            GetComponent<PlayerMov>().enabled = true;
            time = 0f;
        }
    }
}