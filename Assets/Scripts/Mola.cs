using UnityEngine;

public class Mola : MonoBehaviour
{
    public enum Direcao { Cima, DiagonalDireita, DiagonalEsquerda }
    public Direcao direcao = Direcao.Cima;

    public float forca = 20f;
    public AudioClip somMola; // opcional

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direcaoImpulso = Vector2.up;

                switch (direcao)
                {
                    case Direcao.Cima:
                        direcaoImpulso = Vector2.up;
                        break;
                    case Direcao.DiagonalDireita:
                        direcaoImpulso = (Vector2.up + Vector2.right).normalized;
                        break;
                    case Direcao.DiagonalEsquerda:
                        direcaoImpulso = (Vector2.up + Vector2.left).normalized;
                        break;
                }

                rb.linearVelocity = Vector2.zero; // zera antes de aplicar
                rb.AddForce(direcaoImpulso * forca, ForceMode2D.Impulse);

                if (somMola != null)
                    AudioSource.PlayClipAtPoint(somMola, transform.position);
            }
        }
    }
}
