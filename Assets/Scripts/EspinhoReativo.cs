using UnityEngine;
using System.Collections;

public class EspinhoReativo : MonoBehaviour
{
    [Header("Tempo de Anima��o")]
    [SerializeField] private float tempoSubida = 0.5f;
    [SerializeField] private float tempoAtivo = 1.0f;
    [SerializeField] private float tempoDescida = 0.5f;
    [SerializeField] private float tempoInativo = 2.0f;
    [SerializeField] private float delaySeguranca = 0.2f; // Novo: Delay para evitar falsos positivos

    [Header("Velocidade da Anima��o")]
    [SerializeField] private float velocidadeAnimacao = 1f;

    private Animator animator;
    private Collider2D colliderEspinho;
    private bool estaAtivo = false;
    private bool emTransicao = false; // Novo: Estado de transi��o
    private string tagOriginal;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliderEspinho = GetComponent<Collider2D>();
        animator.speed = velocidadeAnimacao;
        tagOriginal = "Ground";
        gameObject.tag = tagOriginal;
        StartCoroutine(ControlarEspinho());
    }

    IEnumerator ControlarEspinho()
    {
        while (true)
        {
            // Estado: Subindo (transi��o)
            emTransicao = true;
            animator.SetTrigger("Subir");
            yield return new WaitForSeconds(tempoSubida);

            // Delay de seguran�a antes de ativar
            yield return new WaitForSeconds(delaySeguranca);

            // Estado: Totalmente Ativo
            emTransicao = false;
            estaAtivo = true;
            gameObject.tag = "Spike";
            colliderEspinho.enabled = true;

            yield return new WaitForSeconds(tempoAtivo);

            yield return new WaitForSeconds(delaySeguranca);

            // Estado: Descendo (transi��o)
            emTransicao = true;
            estaAtivo = false;
            animator.SetTrigger("Descer");
            colliderEspinho.enabled = false; // Desativa imediatamente
            gameObject.tag = tagOriginal;

            yield return new WaitForSeconds(tempoDescida);
            yield return new WaitForSeconds(delaySeguranca);

            // Estado: Inativo
            emTransicao = false;
            yield return new WaitForSeconds(tempoInativo);
            yield return new WaitForSeconds(delaySeguranca);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // S� causa dano se estiver totalmente ativo (n�o em transi��o)
        if (!estaAtivo || emTransicao || !colliderEspinho.enabled) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Dano>(out Dano dano))
            {
                dano.TomarDano(10, gameObject);
            }
        }
    }
}