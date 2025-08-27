using System.Collections;
using UnityEngine;

public class FimDoTutorial : MonoBehaviour
{
    private Dano danoScript;
    private PlayerMov playerMov;

    public GameObject avisoFaltaCaixaUI;
    public GameObject clienteEmojiUI;
    public Sprite emojiFeliz;
    [SerializeField] private TutorialFim tutorialFim; // Referência direta
    [SerializeField] private GameObject Canvas;

    void Start()
    {
        danoScript = Object.FindFirstObjectByType<Dano>();
        playerMov = Object.FindFirstObjectByType<PlayerMov>();

        if (clienteEmojiUI != null)
            clienteEmojiUI.SetActive(false);

        if (tutorialFim != null)
            tutorialFim.painelFimTutorial.SetActive(false);
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

                Debug.Log("A entrega não foi feita! Volte e recupere a caixa.");
                StartCoroutine(ResetarEntrada());
                return;
            }

            Canvas.SetActive(false);
            StartCoroutine(ReacaoClienteEFim());
        }
        if (other.CompareTag("Player"))
        {
            if (playerMov != null)
            {
                playerMov.PararSomCorrida();
            }
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

    private IEnumerator ReacaoClienteEFim()
    {
        Time.timeScale = 0f;

        if (playerMov != null)
        {
            playerMov.enabled = false;
            playerMov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            playerMov.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (clienteEmojiUI != null)
        {
            clienteEmojiUI.SetActive(true);

            var emojiRenderer = clienteEmojiUI.GetComponent<SpriteRenderer>();
            if (emojiRenderer != null)
                emojiRenderer.sprite = emojiFeliz;
        }

        yield return new WaitForSecondsRealtime(2f);

        // chama corretamente o método do TutorialFim
        if (tutorialFim != null)
        {
            tutorialFim.MostrarFimTutorial();
        }
        else
        {
            Debug.LogWarning("TutorialFim não atribuído no FimDoTutorial!");
        }
    }
}
