using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using Unity.VisualScripting;

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
    private PlayerMov Direcao;
    [HideInInspector] public bool CaixaPega;

    public bool caixaInstanciada = false;
    [HideInInspector] public bool CaixaIndoEsquerda, CaixaIndoDireita;

    [Range(0, 100)]
    public float qualidadeEntrega = 100f;
    AudioSource sound;
    public AudioClip caixa_som;

    private static readonly string[] obstaculosQueCaemCaixa = {
        "Spike", "Tatu", "RaizRotatoria", "Passaro", "Meteorito", "PlataformaReativa"
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
        Direcao = Sprite_Dog_Caixa_Normal_0.GetComponent<PlayerMov>();
        sound = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Player != null && Caixa_Separada_0 != null)
        {
            time += Time.deltaTime;
            if (time < 1.2f)
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
            CaixaPega = true;
            sound.clip = caixa_som;
            sound.Play();
        }
    }

    public void DerrubarCaixa()
    {
        if (caixaPrefab != null && !caixaInstanciada)
        {
            caixaInstanciada = true;
            StartCoroutine(Delaycriarcaixa());
        }
    }

    public void ReduzirQualidade(float dano)
    {
        qualidadeEntrega -= dano;
        qualidadeEntrega = Mathf.Clamp(qualidadeEntrega, 0f, 100f);
        Debug.Log("📦 Qualidade da entrega atual: " + qualidadeEntrega);
    }
    public void Criarcaixa()
    {
        if (bool_script != null && bool_script.isInvincible == false)
        {
            Caixa_Separada_0 = Instantiate(caixaPrefab, Player.transform.position, Quaternion.identity);
            Rigidbody2D caixaRb = Caixa_Separada_0.GetComponent<Rigidbody2D>();
            Debug.Log("Caixa instanciada");
            if (caixaRb != null && Direcao.IndoDireita)
            {
                caixaRb.linearVelocity = new Vector2(-move * speed, caixaRb.linearVelocity.y);
                caixaRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                CaixaIndoEsquerda = true;
            }
            if (caixaRb != null && Direcao.IndoEsquerda)
            {
                caixaRb.linearVelocity = new Vector2(move * speed, caixaRb.linearVelocity.y);
                caixaRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                CaixaIndoDireita = true;
            }
        }
    }
    private IEnumerator Delaycriarcaixa()
    {
        yield return new WaitForSeconds(0.15f);
        CaixaPega = false;
        Criarcaixa();
    }
}