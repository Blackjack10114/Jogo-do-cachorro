using UnityEngine;

public class EspinhoReativo : MonoBehaviour
{
    public bool estaAtivo = false;

    // Função chamada quando o espinho atingir o estado ativo
    public void AtivarEspinho()
    {
        estaAtivo = true;
        Debug.Log("Espinho ativado");
    }

    // Função chamada quando o espinho atingir o estado inativo
    public void DesativarEspinho()
    {
        estaAtivo = false;
        Debug.Log("Espinho Desativado");
    }

 
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Colidiu com " + other.name);
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
