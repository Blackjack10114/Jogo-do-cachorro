using UnityEngine;
using System.Collections;

public class Mola : MonoBehaviour
{
    public enum Direcao { Cima, DiagonalDireita, DiagonalEsquerda }
    public Direcao direcao = Direcao.Cima;

    [Header("Forças aplicadas")]
    public float forcaVertical = 15f;
    public float forcaHorizontal = 10f;

    [Header("Configurações Físicas")]
    public bool ignorarTodasAsForcas = true; // Novo: Remove forças residuais

    [Header("Efeitos")]
    public AudioClip somMola;
    public ParticleSystem efeitoVisual;
    private GameObject Player = null;
    private Jump VerificarChao;
    private bool Permitirverchao = false;
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        VerificarChao = Player.GetComponent<Jump>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.GetComponent<PlayerMov>().enabled = false;
            StartCoroutine(Delayverificarchao());
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // 1. Remove TODAS as forças e reseta a física completamente
                if (ignorarTodasAsForcas)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.angularVelocity = 0f;
                    rb.Sleep(); // "Adormece" o Rigidbody para resetar cálculos internos
                    rb.WakeUp(); // Reativa para aplicar o impulso limpo
                }

                // 2. Calcula o impulso
                Vector2 impulso = CalcularImpulso();

                // 3. Aplica a força (usando VelocityChange para ignorar massa e física)
                rb.AddForce(impulso, ForceMode2D.Impulse);

                // 4. Efeitos
                if (somMola != null)
                    AudioSource.PlayClipAtPoint(somMola, transform.position);

                if (efeitoVisual != null)
                    efeitoVisual.Play();
            }
        }
    }

    private Vector2 CalcularImpulso()
    {
        switch (direcao)
        {
            case Direcao.DiagonalDireita:
                return new Vector2(forcaHorizontal, forcaVertical);
            case Direcao.DiagonalEsquerda:
                return new Vector2(-forcaHorizontal, forcaVertical);
            default: // Cima
                return new Vector2(0f, forcaVertical);
        }
    }
    private IEnumerator Delayverificarchao()
    {
        yield return new WaitForSeconds(0.3f);
        Permitirverchao = true;
    }
    private void Update()
    {
        if (Permitirverchao == true)
        {
            if (VerificarChao.grounded == true)
            {
                Player.GetComponent<PlayerMov>().enabled = true;
                Permitirverchao = false;
            }
        }
    }
}