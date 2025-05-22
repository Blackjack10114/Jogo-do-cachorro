using UnityEngine;
using System.Collections;

public class Mola : MonoBehaviour
{
    public enum Direcao { Cima, DiagonalDireita, DiagonalEsquerda }
    public Direcao direcao = Direcao.Cima;

    [Header("Forças aplicadas")]
    public float forcaVertical = 15f;
    public float forcaHorizontal = 10f;

    [Header("Configurações Físicas")]
    public bool ignorarTodasAsForcas = true;

    [Header("Efeitos")]
    public AudioClip somMola;
    public ParticleSystem efeitoVisual;

    private GameObject player;
    private Rigidbody2D rb;
    private Jump verificarChao;
    private PlayerMov playerMov;
    private Animator anim;

    private bool permitirVerChao = false;
    private bool preparado = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = player.GetComponent<Rigidbody2D>();
        verificarChao = player.GetComponent<Jump>();
        playerMov = player.GetComponent<PlayerMov>();
        anim = GetComponent<Animator>();

        anim.ResetTrigger("Ativar");
        anim.Play("MolaBase", 0, 0f); //garante que começa no estado base

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !preparado)
        {
            preparado = true;

            // Desativa movimento
            if (playerMov != null)
                playerMov.enabled = false;

            // Inicia animação
            if (anim != null)
                anim.SetTrigger("Ativar");
        }
    }

    // 🔥 Essa função será chamada no frame certo pelo Animator Event
    public void AplicarImpulso()
    {
        if (ignorarTodasAsForcas)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
            rb.WakeUp();
        }

        Vector2 impulso = CalcularImpulso();
        rb.AddForce(impulso, ForceMode2D.Impulse);

        if (somMola != null)
            AudioSource.PlayClipAtPoint(somMola, transform.position);

        if (efeitoVisual != null)
            efeitoVisual.Play();

        StartCoroutine(DelayVerificarChao());
    }

    private Vector2 CalcularImpulso()
    {
        switch (direcao)
        {
            case Direcao.DiagonalDireita:
                return new Vector2(forcaHorizontal, forcaVertical);
            case Direcao.DiagonalEsquerda:
                return new Vector2(-forcaHorizontal, forcaVertical);
            default:
                return new Vector2(0f, forcaVertical);
        }
    }

    private IEnumerator DelayVerificarChao()
    {
        yield return new WaitForSeconds(0.3f);
        permitirVerChao = true;
    }

    private void Update()
    {
        if (permitirVerChao)
        {
            if (verificarChao.grounded)
            {
                playerMov.enabled = true;
                permitirVerChao = false;
                preparado = false;
            }
        }
    }
}
