using UnityEngine;

public class EspinhoReativo : MonoBehaviour
{
    [SerializeField] private float tempoAtivo = 1.5f;     // tempo que fica com espinho pra fora
    [SerializeField] private float tempoInativo = 2.0f;    // tempo que fica retraído

    private Animator animator;
    private bool estaAtivo = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ControlarEspinho());
    }

    System.Collections.IEnumerator ControlarEspinho()
    {
        while (true)
        {
            estaAtivo = true;
            animator.SetBool("estaAtivo", true);
            Debug.Log("Espinho ativado");
            yield return new WaitForSeconds(tempoAtivo);

            estaAtivo = false;
            animator.SetBool("estaAtivo", false);
            Debug.Log("Espinho desativado");
            yield return new WaitForSeconds(tempoInativo);
        }
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
