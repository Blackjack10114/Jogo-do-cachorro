using UnityEngine;
using System.Collections;

public class EspinhoReativo : MonoBehaviour
{
    [Header("Tempo de Animação")]
    [SerializeField] private float tempoSubida = 0.5f;
    [SerializeField] private float tempoAtivo = 1.0f;
    [SerializeField] private float tempoDescida = 0.5f;
    [SerializeField] private float tempoInativo = 2.0f;

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

        tagOriginal = "PlataformaReativa"; // Define sua tag original
        gameObject.tag = tagOriginal;

        StartCoroutine(ControlarEspinho());
    }

    IEnumerator ControlarEspinho()
    {
        while (true)
        {
            // Subida
            animator.SetTrigger("Subir");
            yield return new WaitForSeconds(tempoSubida);

            // Ativo
            estaAtivo = true;
            gameObject.tag = "Spike";
            colliderEspinho.enabled = true; // Ativa o Collider
            yield return new WaitForSeconds(tempoAtivo);

            // Descida
            estaAtivo = false;
            animator.SetTrigger("Descer");
            gameObject.tag = tagOriginal;
            colliderEspinho.enabled = false; // Desativa o Collider
            yield return new WaitForSeconds(tempoDescida);

            // Inativo
            yield return new WaitForSeconds(tempoInativo);
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
}