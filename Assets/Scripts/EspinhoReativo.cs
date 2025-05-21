using UnityEngine;
using System.Collections;

public class EspinhoReativo : MonoBehaviour
{
    [Header("Velocidade da Anima��o")]
    [SerializeField] private float velocidadeAnimacao = 1f;

    private Animator animator;
    private Collider2D colliderEspinho; // Refer�ncia ao Collider2D
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
            yield return new WaitForSeconds(0.5f);


            // Ativo
            estaAtivo = true;
            gameObject.tag = "Spike";
            colliderEspinho.enabled = true; // Ativa o Collider
            yield return new WaitForSeconds(1f);

            // Descida
            estaAtivo = false;
            animator.SetTrigger("Descer");
            gameObject.tag = tagOriginal;
            colliderEspinho.enabled = false; // Desativa o Collider
            yield return new WaitForSeconds(0.5f);

            // Inativo
            yield return new WaitForSeconds(2f);

        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se est� ativo (redundante, mas extra seguro)
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