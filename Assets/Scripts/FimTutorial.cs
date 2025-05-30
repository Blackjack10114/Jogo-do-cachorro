using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDoTutorial : MonoBehaviour
{
    
    private Dano danoScript;
    private PlayerMov playerMov;

    [SerializeField] private string ProximaCena;
    public GameObject avisoFaltaCaixaUI;
    public GameObject clienteEmojiUI;
    public Sprite emojiFeliz;
    void Start()
    {
       
        danoScript = Object.FindFirstObjectByType<Dano>();
        playerMov = Object.FindFirstObjectByType<PlayerMov>();

        if (clienteEmojiUI != null)
            clienteEmojiUI.SetActive(false); // Esconde o emoji inicialmente
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var anim = other.GetComponent<Animator>();

            if (anim != null && anim.GetBool("ComCaixa") == false)
            {
                if (avisoFaltaCaixaUI != null)
                    avisoFaltaCaixaUI.SetActive(true);

                Debug.Log(" A entrega não foi feita! Volte e recupere a caixa.");
                StartCoroutine(ResetarEntrada());
                return;
            }

           

         

          

            // Começa a sequência da reação do cliente
            StartCoroutine(ReacaoClienteENextScene());
        }
    }

    private IEnumerator ResetarEntrada()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSecondsRealtime(2f);

        if (avisoFaltaCaixaUI != null)
            avisoFaltaCaixaUI.SetActive(false);

        GetComponent<Collider2D>().enabled = true;
    }

    private IEnumerator ReacaoClienteENextScene()
    {
        Time.timeScale = 0f;
        // Para o movimento do player
        if (playerMov != null)
        {
            playerMov.enabled = false;
            playerMov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            playerMov.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Ativa o cliente/emoji
        if (clienteEmojiUI != null)
        {
            clienteEmojiUI.SetActive(true);

            // Define o emoji pela nota
            var emojiRenderer = clienteEmojiUI.GetComponent<SpriteRenderer>();
            if (emojiRenderer != null)
            {
                emojiRenderer.sprite = emojiFeliz;
            }
        }

        // Espera 2 segundos para mostrar a reação
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;
        GerenciadorProgresso.RegistrarCenaAtual(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(ProximaCena);
    }
}
