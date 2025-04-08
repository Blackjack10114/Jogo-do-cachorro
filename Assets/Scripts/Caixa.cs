using UnityEngine;

public class Caixa : MonoBehaviour
{
    private GameObject Player = null;
    private GameObject caixaPrefab = null;
    private GameObject Caixa_Separada_0 = null;
    public float speed, move;
    private Rigidbody2D rb;
    [SerializeField] public float jumpforce;
    private float time = 0f;
    public Sprite Sprite_Dog_Caixa_Normal;
    private Dano bool_script;
    public GameObject Sprite_Dog_Caixa_Normal_0;

    public bool caixaInstanciada = false;

    // 🆕 Variável de qualidade da caixa (de 100 a 0)
    [Range(0, 100)]
    public float qualidadeEntrega = 100f;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        caixaPrefab = Resources.Load<GameObject>("Caixa_Separada_0");
        rb = Player.GetComponent<Rigidbody2D>();
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<Dano>();
    }

    void Update()
    {
        if (Player != null && Caixa_Separada_0 != null)
        {
            time += Time.deltaTime;
            if (time < 0.5f)
            {
                Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Caixa_Separada_0.GetComponent<Collider2D>());
            }
            else
            {
                Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Caixa_Separada_0.GetComponent<Collider2D>(), false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spike"))
        {
            if (caixaPrefab != null && !caixaInstanciada)
            {
                if (bool_script != null && bool_script.isInvincible == false)
                {
                    Caixa_Separada_0 = Instantiate(caixaPrefab, Player.transform.position, Quaternion.identity);
                    Rigidbody2D caixaRb = Caixa_Separada_0.GetComponent<Rigidbody2D>();

                    if (caixaRb != null)
                    {
                        caixaInstanciada = true;
                        caixaRb.linearVelocity = new Vector2(-move * speed, caixaRb.linearVelocity.y);
                        caixaRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                    }
                }
            }
        }

        if (collision.gameObject.CompareTag("caixa"))
        {
            Destroy(Caixa_Separada_0);
            time = 0;
            caixaInstanciada = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Caixa_Normal;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spike"))
        {
            if (caixaPrefab != null && !caixaInstanciada)
            {
                if (bool_script != null && bool_script.isInvincible == false)
                {
                    Caixa_Separada_0 = Instantiate(caixaPrefab, Player.transform.position, Quaternion.identity);
                    Rigidbody2D caixaRb = Caixa_Separada_0.GetComponent<Rigidbody2D>();

                    if (caixaRb != null)
                    {
                        caixaInstanciada = true;
                        caixaRb.linearVelocity = new Vector2(-move * speed, caixaRb.linearVelocity.y);
                        caixaRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    // 🆕 Método para reduzir qualidade da caixa
    public void ReduzirQualidade(float dano)
    {
        qualidadeEntrega -= dano;
        qualidadeEntrega = Mathf.Clamp(qualidadeEntrega, 0f, 100f);
        Debug.Log("📦 Qualidade da entrega atual: " + qualidadeEntrega);
    }
}
