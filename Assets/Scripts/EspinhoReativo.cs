using UnityEngine;
using System.Collections;

public class EspinhoReativo : MonoBehaviour
{
    [Header("Velocidade da Animação")]
    [SerializeField] private float velocidadeAnimacao = 1f;

    private Animator animator;
    private Collider2D colliderEspinho; // Referência ao Collider2D
    private bool estaAtivo = false;
    private string tagOriginal;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliderEspinho = GetComponent<Collider2D>(); // Pega o Collider2D
        animator.speed = velocidadeAnimacao;

        tagOriginal = "Ground"; // Define sua tag original
        gameObject.tag = tagOriginal;

        StartCoroutine(ControlarEspinho());
    }

    IEnumerator ControlarEspinho()
    {
        while (true)
        {
            // Subida
            estaAtivo = false;
            gameObject.tag = tagOriginal;
            colliderEspinho.enabled = false;
            animator.SetTrigger("Subir");
            yield return new WaitForSeconds(0.5f); // Espera a animação de subida

            // Ativo
            estaAtivo = true;
            gameObject.tag = "Spike";
            colliderEspinho.enabled = true;
            yield return new WaitForSeconds(1f); // Tempo ativo

            // Descida
            estaAtivo = false;
            animator.SetTrigger("Descer");
            gameObject.tag = tagOriginal;
            colliderEspinho.enabled = false;
            yield return new WaitForSeconds(0.5f); // Espera a animação de descida

            // Inativo
            yield return new WaitForSeconds(2f); // Tempo inativo antes de subir de novo
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se está ativo (redundante, mas extra seguro)
        if (!estaAtivo || !colliderEspinho.enabled) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Dano>(out Dano dano))
            {
                dano.TomarDano(10, gameObject);
            }
        }
    }
    public void AtivarDano()
    {
        estaAtivo = true;
        gameObject.tag = "Spike";
        colliderEspinho.enabled = true;
    }

    public void DesativarDano()
    {
        estaAtivo = false;
        gameObject.tag = tagOriginal;
        colliderEspinho.enabled = false;
    }

}