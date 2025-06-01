using System.Collections;
using UnityEngine;

public class FimDoTutorial : MonoBehaviour
{
    private Dano danoScript;
    private PlayerMov playerMov;

    public GameObject avisoFaltaCaixaUI;
    public GameObject clienteEmojiUI;
    public Sprite emojiFeliz;
    public GameObject painelFimTutorial;

    void Start()
    {
        danoScript = Object.FindFirstObjectByType<Dano>();
        playerMov = Object.FindFirstObjectByType<PlayerMov>();

        if (clienteEmojiUI != null)
            clienteEmojiUI.SetActive(false);

        if (painelFimTutorial != null)
            painelFimTutorial.SetActive(false);
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

            StartCoroutine(ReacaoClienteEFim());
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
        // Para o movimento do jogador
        if (playerMov != null)
        {
            playerMov.enabled = false;
            playerMov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            playerMov.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Mostra o emoji
        if (clienteEmojiUI != null)
        {
            clienteEmojiUI.SetActive(true);

            var emojiRenderer = clienteEmojiUI.GetComponent<SpriteRenderer>();
            if (emojiRenderer != null)
                emojiRenderer.sprite = emojiFeliz;
        }

        // Espera 2 segundos
        yield return new WaitForSecondsRealtime(2f);

        // Mostra o painel de fim de tutorial
        if (painelFimTutorial != null)
            painelFimTutorial.SetActive(true);
        
    }
}
