using UnityEngine;
using UnityEngine.Rendering;

public class coletavel : MonoBehaviour
{
    public bool osso_coletado = false;
    private GameObject Player = null;
    public GameObject Ossinho;

    public AudioClip somDaColeta;
    private AudioSource audioSource;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (somDaColeta != null && audioSource != null)
            {
                // PlayOneShot é ideal para efeitos sonoros
                audioSource.PlayOneShot(somDaColeta);
            }

            osso_coletado = true;
            Debug.Log("osso coletado!");

            // Adiciona pontuação
            var pontuacao = FindAnyObjectByType<SistemaPontuacao>();
            if (pontuacao != null) pontuacao.AdicionarOsso();


            GetComponent<Renderer>().enabled = false;
            Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Ossinho.GetComponent<Collider2D>());
        }

    }
}
