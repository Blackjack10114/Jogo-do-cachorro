using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlataformaMovel : MonoBehaviour
{
    public Vector3 direcao = Vector3.right;
    public float distancia = 3f;
    public float velocidade = 2f;

    private Vector3 posInicial;
    private Rigidbody2D rb;
    private float tempo;

    [HideInInspector] public Vector3 deltaMovimento;
    private Vector3 ultimaPos;

    public Vector3 ultimaPosicao;
    public Vector3 velocidadeCalculada;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posInicial = transform.position;
        ultimaPosicao = transform.position;
    }

    void FixedUpdate()
    {
        tempo += Time.fixedDeltaTime;
        float movimento = Mathf.PingPong(tempo * velocidade, distancia);
        Vector3 novaPos = posInicial + direcao.normalized * movimento;
        rb.MovePosition(novaPos);

        velocidadeCalculada = (transform.position - ultimaPosicao) / Time.fixedDeltaTime;
        ultimaPosicao = transform.position;
    }
}

