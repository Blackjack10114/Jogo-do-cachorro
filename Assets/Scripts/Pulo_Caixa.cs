using UnityEngine;

public class Pulo_Caixa : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] public float jumpforce;
    public float speed, move;
    private Caixa direcao;
    private static readonly string[] obstaculosQuePulamcaixa = {
        "Spike", "Buraco", "Tatu", "RaizRotatoria", "Passaro", "Meteorito"
    };
    private GameObject Player = null;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindWithTag("Player");
        direcao = Player.GetComponent<Caixa>();
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
