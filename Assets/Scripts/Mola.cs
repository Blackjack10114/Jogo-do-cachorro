using UnityEngine;

public class Mola : MonoBehaviour
{
    public enum Direcao { Cima, DiagonalDireita, DiagonalEsquerda }
    public Direcao direcao = Direcao.Cima;

    [Header("Forças aplicadas")]
    public float forcaVertical = 15f;
    public float forcaHorizontal = 10f;

    public AudioClip somMola;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 impulso = Vector2.zero;

                switch (direcao)
                {
                    case Direcao.Cima:
                        impulso = new Vector2(0f, forcaVertical);
                        break;
                    case Direcao.DiagonalDireita:
                        impulso = new Vector2(forcaHorizontal, forcaVertical);
                        break;
                    case Direcao.DiagonalEsquerda:
                        impulso = new Vector2(-forcaHorizontal, forcaVertical);
                        break;
                }

                rb.linearVelocity = Vector2.zero; // zera antes de aplicar
                rb.AddForce(impulso, ForceMode2D.Impulse);

                if (somMola != null)
                    AudioSource.PlayClipAtPoint(somMola, transform.position);
            }
        }
    }
}
