using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class EspinhoReativo : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float velocidadeAnimacao = 1f;
    

    [Header("Tempos Base (ajustados pela velocidade)")]
    [SerializeField] private float tempoSubidaBase = 0.5f;
    [SerializeField] private float tempoAtivoBase = 1f;
    [SerializeField] private float tempoDescidaBase = 0.5f;
    [SerializeField] private float tempoInativoBase = 2f;

    // Componentes
    private Animator animator;
    private Collider2D colliderEspinho;

    // Estados
    private enum Estado { Inativo, Subindo, Ativo, Descendo }
    private Estado estadoAtual;
    private string tagOriginal;

    public AudioClip somAbrir;
    public AudioClip somFechar;
    public AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliderEspinho = GetComponent<Collider2D>();
        animator.speed = velocidadeAnimacao;
        tagOriginal = "Ground";
        estadoAtual = Estado.Inativo;

        // Inicia desativado
        DesativarDano();

        StartCoroutine(CicloEspinho());
    }

    IEnumerator CicloEspinho()
    {
        while (true)
        {
            // Subida
            estadoAtual = Estado.Subindo;
            animator.SetBool("Subindo", true);
            animator.SetBool("Descendo", false);
            yield return new WaitForSeconds(tempoSubidaBase / velocidadeAnimacao);

            // Ativo
            estadoAtual = Estado.Ativo;
            animator.SetBool("Subindo", false);
            AtivarDano();
            yield return new WaitForSeconds(tempoAtivoBase / velocidadeAnimacao);

            // Descida
            estadoAtual = Estado.Descendo;
            animator.SetBool("Descendo", true);
            DesativarDano();
            yield return new WaitForSeconds(tempoDescidaBase / velocidadeAnimacao);

            // Inativo
            estadoAtual = Estado.Inativo;
            animator.SetBool("Descendo", false);
            yield return new WaitForSeconds(tempoInativoBase / velocidadeAnimacao);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Só causa dano se estiver no estado ATIVO
        if (estadoAtual != Estado.Ativo) return;

        if (collision.gameObject.CompareTag("Player") &&
            collision.gameObject.TryGetComponent<Dano>(out Dano dano))
        {
            dano.TomarDano(10, gameObject);
        }
    }

    void AtivarDano()
    {
        gameObject.tag = "Spike";
        colliderEspinho.enabled = true;
    }

    void DesativarDano()
    {
        gameObject.tag = tagOriginal;
        colliderEspinho.enabled = false;
    }

    public void SomAbrir()
    {
        audioSource.PlayOneShot(somAbrir);
    }

    public void SomFechar()
    {
        audioSource.PlayOneShot(somFechar);
    }



}