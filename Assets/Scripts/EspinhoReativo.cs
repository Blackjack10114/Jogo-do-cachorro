using UnityEngine;

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
    private bool estaAtivo = false;
    private string tagOriginal;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = velocidadeAnimacao;
        tagOriginal = gameObject.tag;
        StartCoroutine(ControlarEspinho());
    }

    System.Collections.IEnumerator ControlarEspinho()
    {
        // Subida
        animator.SetTrigger("Subir");
        yield return new WaitForSeconds(tempoSubida);

        // Ativo
        estaAtivo = true;
        gameObject.tag = "Spike"; // Tag temporária ativa
        yield return new WaitForSeconds(tempoAtivo);

        // Descida
        estaAtivo = false;
        animator.SetTrigger("Descer");
        gameObject.tag = tagOriginal; // Volta à tag original
        yield return new WaitForSeconds(tempoDescida);

        // Inativo
        yield return new WaitForSeconds(tempoInativo);

        StartCoroutine(ControlarEspinho());
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!estaAtivo) return;

        if (other.CompareTag("Player"))
        {
            Dano dano = other.GetComponent<Dano>();
            if (dano != null)
            {
                dano.TomarDano(10, gameObject);
            }
        }
    }
}
