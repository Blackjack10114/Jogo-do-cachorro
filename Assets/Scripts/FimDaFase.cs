using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FimDaFase : MonoBehaviour
{
    private SistemaPontuacao pontuacaoScript;
    private Dano danoScript;
    private PlayerMov playerMov;
    private GameObject Player;
    public GameObject Caixa;

    [SerializeField] private string cenaFim;
    public GameObject avisoFaltaCaixaUI;
    public GameObject clienteEmojiUI;
    public Sprite emojiFeliz;
    public Sprite emojiNeutro;
    public Sprite emojiBravo;
    string sceneName;
    private bool estatatu, estadino, estaalien, Terminouanimacao;
    public Sprite Sprite_Dog_Sem_Caixa;
    private Vector3 offset;
    public Transform destino, destino2;
    public float tempoDeMovimento = 2f;
    private bool Terminou_fase, naoemoji;

    void Start()
    {
        pontuacaoScript = Object.FindFirstObjectByType<SistemaPontuacao>();
        danoScript = Object.FindFirstObjectByType<Dano>();
        playerMov = Object.FindFirstObjectByType<PlayerMov>();

        if (clienteEmojiUI != null)
            clienteEmojiUI.SetActive(false); // Esconde o emoji inicialmente
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        Player = GameObject.FindWithTag("Player");
        offset = new Vector3(0f, 1.5f, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var anim = other.GetComponent<Animator>();
        if (other.CompareTag("Player"))
        {
            if (anim.GetBool("ComCaixa") == false)
            {
               // if (avisoFaltaCaixaUI != null)
               //     avisoFaltaCaixaUI.SetActive(true);

                Debug.Log(" A entrega não foi feita! Volte e recupere a caixa.");
               // StartCoroutine(ResetarEntrada());
                return;
            }
            if (estatatu)
            {
                Comecar_animacao();
            }

                pontuacaoScript.CalcularPontuacaoFinal();

            int pontos = pontuacaoScript.GetPontuacaoFinalNumerica();
            string nota = pontuacaoScript.GetClassificacaoLetra();

            PlayerPrefs.SetInt("PontuacaoFinal", pontos);
            PlayerPrefs.SetString("NotaFinal", nota);
            if (estadino)
            {
                StartCoroutine(ReacaoClienteENextScene(nota));
            }
            if (estaalien)
            {
                StartCoroutine(ReacaoClienteENextScene(nota));
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

    private IEnumerator ReacaoClienteENextScene(string nota)
    {
        Time.timeScale = 0f;
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
    private void Update()
    {
        verificar_cena();
        if (Terminou_fase && !naoemoji)
        {
            string nota = pontuacaoScript.GetClassificacaoLetra();
            StartCoroutine(ReacaoClienteENextScene(nota));
        }
    }
    private void verificar_cena()
    {
        if (sceneName == "Fase_TatuMafioso_01")
        {
            estatatu = true;
        }
        else if (sceneName == "Fase_Alien_02")
        {
            estaalien = true;
        }
        else if (sceneName == "Fase_Dino_03")
        {
            estadino = true;
        }
    }
    private void Comecar_animacao()
    {
        Time.timeScale = 0f;
        if (estatatu)
        {
            Player.transform.position = new Vector3(1267f, -13.7f, 0f);
            if (playerMov != null)
            {
                playerMov.enabled = false;
                playerMov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                playerMov.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            StartCoroutine(Delay_tirar_caixa());
        }
    }
    private IEnumerator Delay_tirar_caixa()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Player.GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;
        avisoFaltaCaixaUI = Instantiate(avisoFaltaCaixaUI, Player.transform.position + offset, Quaternion.identity);
        avisoFaltaCaixaUI.transform.localScale = new Vector3(2, 2, 2);
        StartCoroutine(MoverAteDestino());
    }
    IEnumerator MoverAteDestino()
    {
        Vector2 posInicial = avisoFaltaCaixaUI.transform.position;
        Vector2 posFinal = destino.position;
        float tempoPassado = 0f;

        while (tempoPassado < tempoDeMovimento)
        {
            tempoPassado += Time.unscaledDeltaTime;
            float t = tempoPassado / tempoDeMovimento;
            avisoFaltaCaixaUI.transform.position = Vector2.Lerp(posInicial, posFinal, t);
            yield return null;
        }

        avisoFaltaCaixaUI.transform.position = posFinal;
        Destroy(avisoFaltaCaixaUI);
        StartCoroutine(DelayInstanciarCaixa());
    }
    private IEnumerator DelayInstanciarCaixa()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Caixa = Instantiate(Caixa, new Vector3 (1283f, 20f, 0f) , Quaternion.identity);
        StartCoroutine(MoverAteDestino2());
    }
    private IEnumerator MoverAteDestino2()
    {
        Vector2 posInicial = Caixa.transform.position;
        Vector2 posFinal = destino2.position;
        float tempoPassado = 0f;

        while (tempoPassado < tempoDeMovimento)
        {
            tempoPassado += Time.unscaledDeltaTime;
            float t = tempoPassado / tempoDeMovimento;
            Caixa.transform.position = Vector2.Lerp(posInicial, posFinal, t);
            yield return null;
        }
        Caixa.transform.position = posFinal;
        Terminou_fase = true;
    }
}
