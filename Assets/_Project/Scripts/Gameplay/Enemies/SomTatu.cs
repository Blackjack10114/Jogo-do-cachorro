using UnityEngine;

public class ParedeSomTatu : MonoBehaviour
{
    public AudioSource somDeBatida; // atribua no Inspector ou use GetComponent

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Tatu")) // Certifique-se que o tatu tem a tag "Tatu"
        {
            if (somDeBatida != null)
                somDeBatida.Play();
        }
    }
}
