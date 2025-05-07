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

    [Range(0, 100)]
    public float qualidadeEntrega = 100f;

    private static readonly string[] obstaculosQueCaemCaixa = {
        "Spike", "Tatu", "RaizRotatoria", "Passaro", "Meteorito"
    };
    private bool TagCaiCaixa(string tagcaixa)
    {
        foreach (string t in obstaculosQueCaemCaixa)
        {
            if (tagcaixa == t) return true;
        }
        return false;
    }


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
        if (TagCaiCaixa(collision.gameObject.tag))
        {
            DerrubarCaixa();
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
        if (other.CompareTag("Spike") || other.CompareTag("Tatu"))
        {
            DerrubarCaixa();
        }
    }


    public void DerrubarCaixa()
    {
        if (caixaPrefab != null && !caixaInstanciada)
        {
            
            {
                Caixa_Separada_0 = Instantiate(caixaPrefab, Player.transform.position, Quaternion.identity);
                Rigidbody2D caixaRb = Caixa_Separada_0.GetComponent<Rigidbody2D>();
                Debug.Log("Caixa instanciada");
                if (caixaRb != null)
                {
                    caixaInstanciada = true;
                    caixaRb.linearVelocity = new Vector2(-move * speed, caixaRb.linearVelocity.y);
                    caixaRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                }
            }
        }
    }

    public void ReduzirQualidade(float dano)
    {
        qualidadeEntrega -= dano;
        qualidadeEntrega = Mathf.Clamp(qualidadeEntrega, 0f, 100f);
        Debug.Log("📦 Qualidade da entrega atual: " + qualidadeEntrega);
    }
}
