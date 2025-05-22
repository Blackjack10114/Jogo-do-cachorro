using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDaFase : MonoBehaviour
{
    private SistemaPontuacao pontuacaoScript;
    private Dano danoScript;
    private PlayerMov playerMov;

    [SerializeField] private string cenaFim;
    public GameObject avisoFaltaCaixaUI;
    public GameObject clienteEmojiUI;
    public Sprite emojiFeliz;
    public Sprite emojiNeutro;
    public Sprite emojiBravo;

    void Start()
    {
        pontuacaoScript = Object.FindFirstObjectByType<SistemaPontuacao>();
        danoScript = Object.FindFirstObjectByType<Dano>();
        playerMov = Object.FindFirstObjectByType<PlayerMov>();

        if (clienteEmojiUI != null)
            clienteEmojiUI.SetActive(false); // Esconde o emoji inicialmente
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var spriteAtual = other.GetComponent<SpriteRenderer>().sprite;

            if (spriteAtual == danoScript.Sprite_Dog_Sem_Caixa)
            {
                if (avisoFaltaCaixaUI != null)
                    avisoFaltaCaixaUI.SetActive(true);

                Debug.Log(" A entrega não foi feita! Volte e recupere a caixa.");
                StartCoroutine(ResetarEntrada());
                return;
            }

            pontuacaoScript.CalcularPontuacaoFinal();

            int pontos = pontuacaoScript.GetPontuacaoFinalNumerica();
            string nota = pontuacaoScript.GetClassificacaoLetra();

            PlayerPrefs.SetInt("PontuacaoFinal", pontos);
            PlayerPrefs.SetString("NotaFinal", nota);

            // Começa a sequência da reação do cliente
            StartCoroutine(ReacaoClienteENextScene(nota));
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

    private IEnumerator ReacaoClienteENextScene(string nota)
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
                if (nota == "S+" || nota == "S" || nota == "A")
                    emojiRenderer.sprite = emojiFeliz;
                else if (nota == "B" || nota == "C")
                    emojiRenderer.sprite = emojiNeutro;
                else
                    emojiRenderer.sprite = emojiBravo;
            }
        }

        // Espera 2 segundos para mostrar a reação
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;
        GerenciadorProgresso.RegistrarCenaAtual(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(cenaFim);
    }
}
