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
    public GameObject Doug;
    private Rigidbody2D rb;
    [SerializeField] public float jumpforce;
    private float time = 0f;
    private Dano bool_script;
    private PlayerMov Direcao;
    [HideInInspector] public bool CaixaPega;
    private bool estacomcaixa;

    public bool caixaInstanciada = false;
    [HideInInspector] public bool CaixaIndoEsquerda, CaixaIndoDireita;

    [Range(0, 100)]
    public float qualidadeEntrega = 100f;
    AudioSource sound;
    public AudioClip caixa_som;

    private Animator animDoug;

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
        animDoug = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        caixaPrefab = Resources.Load<GameObject>("Caixa_Separada_0");
        rb = Player.GetComponent<Rigidbody2D>();
        bool_script = Doug.GetComponent<Dano>();
        Direcao = Doug.GetComponent<PlayerMov>();
        sound = gameObject.GetComponent<AudioSource>();
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
        if (collision.gameObject.CompareTag("caixa"))
        {
            pegarcaixa();
        }
        if (bool_script.isInvincible == true) return;

        if (TagCaiCaixa(collision.gameObject.tag))
        {
            DerrubarCaixa();
        }
    }

    public void DerrubarCaixa()
    {
        if (caixaPrefab != null && !caixaInstanciada)
        {
            caixaInstanciada = true;
            CaixaPega = false;
            GetComponent<Animator>().SetBool("ComCaixa", false); // 🔥 Aqui garante que troca para sem caixa
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
            Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Caixa_Separada_0.GetComponent<Collider2D>());
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
    public void pegarcaixa()
    {
        CaixaPega = true;
        Destroy(Caixa_Separada_0);
        time = 0;
        caixaInstanciada = false;
        estacomcaixa = true;
        CaixaPega = true;

        animDoug.SetBool("ComCaixa", true);


        sound.clip = caixa_som;
        sound.Play();
    }
    private IEnumerator Delaycriarcaixa()
    {
        yield return new WaitForSeconds(0.15f);
        CaixaPega = false;
        Criarcaixa();
    }
    private IEnumerator Delayprapegarcaixa()
    {
        yield return new WaitForSeconds(0.3f);
        pegarcaixa();
    }
}