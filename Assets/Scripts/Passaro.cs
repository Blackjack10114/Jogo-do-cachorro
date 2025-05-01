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

        // Destroi o p�ssaro ap�s percorrer a dist�ncia m�xima
        if (Vector3.Distance(posicaoInicial, transform.position) >= distanciaVoo)
        {
            Destroy(gameObject);
        }
    }
}
