using UnityEngine;

public class Passaro : MonoBehaviour
{
    public float velocidade = 5f;
    public float distanciaVoo = 10f;
    public Transform jogador;
    public float distanciaAtivacao = 5f;

    private Vector3 posicaoInicial;
    private bool voando = false;

    void Start()
    {
        posicaoInicial = transform.position;
    }

    void Update()
    {
        // Ativa o voo se o jogador estiver próximo
        if (!voando && Vector3.Distance(jogador.position, transform.position) < distanciaAtivacao)
        {
            voando = true;
        }

        // Se estiver voando, mover em linha reta
        if (voando)
        {
            transform.Translate(Vector2.right * velocidade * Time.deltaTime);

            if (Vector3.Distance(posicaoInicial, transform.position) >= distanciaVoo)
            {
                Destroy(gameObject);
            }
        }
    }
}
