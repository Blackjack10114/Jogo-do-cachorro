using UnityEngine;

public class Passaro : MonoBehaviour
{
    public float velocidade = 5f;
    public float distanciaVoo = 10f;

    [HideInInspector] public Transform jogador; // ainda usado pelo spawner

    private Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // Voa em linha reta para a direita
        transform.Translate(Vector2.right * velocidade * Time.deltaTime);

        // Destroi o pássaro após percorrer a distância máxima
        if (Vector3.Distance(posicaoInicial, transform.position) >= distanciaVoo)
        {
            Destroy(gameObject);
        }
    }
}
