using UnityEngine;

public class Pulo_Caixa : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] public float jumpforce;
    public float speed, move;
    private Caixa direcao;
    private static readonly string[] obstaculosQuePulamcaixa = {
        "Spike", "Tatu", "RaizRotatoria", "Passaro", "Meteorito"
    };
    private GameObject Player = null;
    private Dano VerCaixa;
    public Sprite dog_caixa;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        direcao = Player.GetComponent<Caixa>();
        VerCaixa = GetComponent<Dano>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
       GameObject origem = collision.gameObject;
        if (TagPulaCaixa(origem.tag) && direcao.CaixaIndoEsquerda)
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector2(-move * speed, rb.linearVelocity.y);
        }
        if (TagPulaCaixa(origem.tag) && direcao.CaixaIndoDireita)
        {
            rb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
        }
        if (collision.gameObject.tag == "Buraco")
        {
            Destroy(gameObject);
            direcao.caixaInstanciada = false;
            Player.GetComponent<SpriteRenderer>().sprite = dog_caixa;
        }
    }
    private bool TagPulaCaixa(string tag)
    {
        foreach (string t in obstaculosQuePulamcaixa)
        {
            if (tag == t) return true;
        }
        return false;
    }
    private void Update()
    {
        if (direcao.CaixaPega == true)
        {
            direcao.CaixaIndoDireita = false;
            direcao.CaixaIndoEsquerda = false;
        }
    }
}
