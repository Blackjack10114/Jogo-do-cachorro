using UnityEngine;
using System.Collections;
public class Espada_Fantasma : MonoBehaviour
{
    [Header("Detecção")]
    [SerializeField] private float distanciaDeteccao = 10f;
    [SerializeField] private string playerTag = "Player";

    [Header("Mira")]
    [SerializeField] private float tempoMirando = 1f;

    [Header("Movimento")]
    [SerializeField] private float velocidade = 10f;

    private Transform player;
    private Rigidbody2D rb;

    private Vector2 direcaoAtaque;
    private float timerMira;

    private bool detectouPlayer;
    private bool atacando, Atacou;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        if (!detectouPlayer)
        {
            DetectarPlayer();
        }
        else if (!atacando)
        {
            MirarPlayer();
        }
    }

    void FixedUpdate()
    {
        if (atacando)
        {
            Avancar();
        }
        else
        {
            rb.linearVelocity = direcaoAtaque * velocidade * 0;
        }
    }

    void DetectarPlayer()
    {
        GameObject objPlayer = GameObject.FindGameObjectWithTag(playerTag);
        if (objPlayer == null) return;

        float distancia = Vector2.Distance(transform.position, objPlayer.transform.position);

        if (distancia <= distanciaDeteccao)
        {
            player = objPlayer.transform;
            detectouPlayer = true;
            timerMira = tempoMirando;
        }
    }

    void MirarPlayer()
    {
        if (player == null) return;

        timerMira -= Time.deltaTime;

        direcaoAtaque = ((Vector2)player.position - rb.position).normalized;
 
        float angulo = Mathf.Atan2(direcaoAtaque.y, direcaoAtaque.x) * Mathf.Rad2Deg;
        rb.rotation = angulo;

        if (timerMira <= 0f)
        {
            atacando = true;
            StartCoroutine(Reiniciar());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag))
        {
            Physics2D.IgnoreCollision(
                collision.collider,
                GetComponent<Collider2D>()
            );
        }
        else
        {
            Atacou = true;
        }
        if (Atacou)
        {
                Physics2D.IgnoreCollision(
                collision.collider,
                GetComponent<Collider2D>()
            );
        }
    }

    void Avancar()
    {
        rb.linearVelocity = direcaoAtaque * velocidade;
    }

    // Gizmo para visualizar detecção
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaDeteccao);
    }
    private IEnumerator Reiniciar()
    {
        yield return new WaitForSeconds(3f);
        detectouPlayer = false;
        atacando = false;
        Atacou = false;
    }
}
