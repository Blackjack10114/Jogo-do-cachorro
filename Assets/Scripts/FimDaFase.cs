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
    public GameObject Canvas;

    [SerializeField] private string cenaFim;
    public GameObject avisoFaltaCaixaUI;
    public GameObject clienteEmojiUI;
    public Sprite emojiCoracao;
    public Sprite emojiFeliz;
    public Sprite emojiNeutro;
    public Sprite emojiBravo;
    string sceneName;
    private bool estatatu, estadino, estaalien, Terminouanimacao;
    public Sprite Sprite_Dog_Sem_Caixa;
    private Vector3 offset;
    public Transform destino, destino2;
    public float tempoDeMovimento;
    private bool Terminou_fase, naoemoji;
    public GameObject Comida_fase;
    private GameObject meteorofinal, consumidor;

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
        meteorofinal = GameObject.FindWithTag("MeteoroFinal");
        consumidor = GameObject.FindWithTag("Consumidor");
        Canvas canvas = GetComponent<Canvas>();
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
            if (estaalien)
            {
                Comecar_animacao();
            }
            if (estadino)
            {
                Comecar_animacao();
            }

            pontuacaoScript.CalcularPontuacaoFinal();

            int pontos = pontuacaoScript.GetPontuacaoFinalNumerica();
            string nota = pontuacaoScript.GetClassificacaoLetra();

            PlayerPrefs.SetFloat("VidaFinal", danoScript.pv);
            PlayerPrefs.SetInt("PontuacaoFinal", pontos);
            PlayerPrefs.SetString("NotaFinal", nota);
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
            emojiRenderer.sortingLayerName = "HUD";
            emojiRenderer.sortingOrder = 8;
            if (emojiRenderer != null)
            {
                if (nota == "S+" || nota == "S" )
                    emojiRenderer.sprite = emojiCoracao;
                else if (nota == "A" || nota == "B")
                    emojiRenderer.sprite = emojiFeliz;
                else if (nota == "C" || nota == "D")
                    emojiRenderer.sprite = emojiNeutro;
                else
                    emojiRenderer.sprite = emojiBravo;
            }
        }

        // Espera 2 segundos para mostrar a reação
        yield return new WaitForSecondsRealtime(2f);

        Time.timeScale = 1f;
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
        if (estatatu)
        {
            tempoDeMovimento = 2f;
        }
        if (estaalien)
        {
            tempoDeMovimento = 1.5f;
        }
        if (estadino)
        {
            tempoDeMovimento = 1.3f;
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
        Canvas.SetActive(false);
        if (estatatu)
        {
            Time.timeScale = 0f;
            Player.transform.position = new Vector3(1267f, -13.7f, 0f);
            if (playerMov != null)
            {
              playerMov.enabled = false;
              playerMov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
              playerMov.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            StartCoroutine(Delay_tirar_caixa());
        }
        if (estaalien)
        {
            Time.timeScale = 0f;
            Player.transform.position = new Vector3(672f, 911.3f, 0f);
            if (playerMov != null)
            {
                playerMov.enabled = false;
                playerMov.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                playerMov.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            }
            StartCoroutine(Delay_tirar_caixa());
        }
        if (estadino)
        {
            Time.timeScale = 0f;
            Player.transform.position = new Vector3(1745f, 241.3f, 0f);
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
        if (estatatu)
        {
            Player.GetComponent<Animator>().enabled = false;
            Player.GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;
            avisoFaltaCaixaUI = Instantiate(avisoFaltaCaixaUI, Player.transform.position + offset, Quaternion.identity);
            avisoFaltaCaixaUI.transform.localScale = new Vector3(2, 2, 2);
            StartCoroutine(MoverAteDestino());
        }
        if (estaalien)
        {
            Player.GetComponent<Animator>().enabled = false;
            Player.GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;
            avisoFaltaCaixaUI = Instantiate(avisoFaltaCaixaUI, Player.transform.position + offset, Quaternion.identity);
            avisoFaltaCaixaUI.transform.localScale = new Vector3(2, 2, 2);
            StartCoroutine(MoverAteDestino());
        }
        if (estadino)
        {
            Player.GetComponent<Animator>().enabled = false;
            Player.GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Sem_Caixa;
            avisoFaltaCaixaUI = Instantiate(avisoFaltaCaixaUI, Player.transform.position + offset, Quaternion.identity);
            avisoFaltaCaixaUI.transform.localScale = new Vector3(2, 2, 2);
            StartCoroutine(MoverAteDestino());
        }
    }
    IEnumerator MoverAteDestino()
    {
        if (estatatu)
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
        if (estaalien)
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
            StartCoroutine(MoverAteDestino2());
        }
        if (estadino)
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
    }
    private IEnumerator DelayInstanciarCaixa()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        if (estatatu)
        {
            Caixa = Instantiate(Caixa, new Vector3(1283f, 20f, 0f), Quaternion.identity);
            Caixa.transform.localScale = new Vector3(2, 2, 2);
            StartCoroutine(MoverAteDestino2());
        }
        if (estadino)
        {
            Caixa = Instantiate(Caixa, new Vector3(1695f, 254f, 0f), Quaternion.identity);
            Caixa.transform.localScale = new Vector3(2, 2, 2);
            StartCoroutine(MoverAteDestino2());
        }
    }
    private IEnumerator MoverAteDestino2()
    {
        if (estatatu)
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
            StartCoroutine(Tirarcomida());
        }
        if (estaalien)
        {
            Vector2 posInicial = avisoFaltaCaixaUI.transform.position;
            Vector2 posFinal = destino2.position;
            float tempoPassado = 0f;

            while (tempoPassado < tempoDeMovimento)
            {
                tempoPassado += Time.unscaledDeltaTime;
                float t = tempoPassado / tempoDeMovimento;
                avisoFaltaCaixaUI.transform.position = Vector2.Lerp(posInicial, posFinal, t);
                yield return null;
            }
            avisoFaltaCaixaUI.transform.position = posFinal;
            StartCoroutine(Tirarcomida());
        }
        if (estadino)
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
            StartCoroutine(Tirarcomida());
        }
    }
    private IEnumerator Tirarcomida()
    {
        if (estatatu)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            Vector2 posInicial = Caixa.transform.position;
            Vector2 posFinal = new Vector2(1282.7f, -12f);
            float tempoPassado = 0f;
            Comida_fase = Instantiate(Comida_fase, Caixa.transform.position, Quaternion.identity);
            while (tempoPassado < tempoDeMovimento)
            {
                tempoPassado += Time.unscaledDeltaTime;
                float t = tempoPassado / tempoDeMovimento;
                Comida_fase.transform.position = Vector2.Lerp(posInicial, posFinal, t);
                yield return null;
            }
            Comida_fase.transform.position = posFinal;
            yield return new WaitForSecondsRealtime(1.5f);
            Destroy(Comida_fase);
            yield return new WaitForSecondsRealtime(0.5f);
            Terminou_fase = true;
        }
        if (estaalien)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            Vector2 posInicial = avisoFaltaCaixaUI.transform.position;
            Vector2 posFinal = new Vector2(641.8f, 921f);
            float tempoPassado = 0f;
            Comida_fase = Instantiate(Comida_fase, avisoFaltaCaixaUI.transform.position, Quaternion.identity);
            while (tempoPassado < tempoDeMovimento)
            {
                tempoPassado += Time.unscaledDeltaTime;
                float t = tempoPassado / tempoDeMovimento;
                Comida_fase.transform.position = Vector2.Lerp(posInicial, posFinal, t);
                yield return null;
            }
            Comida_fase.transform.position = posFinal;
            Destroy(Comida_fase);
            yield return new WaitForSecondsRealtime(1f);
            Terminou_fase = true;
        }
        if (estadino)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            Vector2 posInicial = Caixa.transform.position;
            Vector2 posFinal = new Vector2(1767.79f, 244.36f);
            float tempoPassado = 0f;
            Comida_fase = Instantiate(Comida_fase, Caixa.transform.position, Quaternion.identity);
            while (tempoPassado < tempoDeMovimento)
            {
                tempoPassado += Time.unscaledDeltaTime;
                float t = tempoPassado / tempoDeMovimento;
                Comida_fase.transform.position = Vector2.Lerp(posInicial, posFinal, t);
                yield return null;
            }
            Comida_fase.transform.position = posFinal;
            Destroy(Comida_fase);
            yield return new WaitForSecondsRealtime(0.5f);
            string nota = pontuacaoScript.GetClassificacaoLetra();
            StartCoroutine(reacaodino(nota));
        }
    }
    private IEnumerator reacaodino(string nota)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (clienteEmojiUI != null)
        {
            clienteEmojiUI.SetActive(true);

            // Define o emoji pela nota
            var emojiRenderer = clienteEmojiUI.GetComponent<SpriteRenderer>();
            if (emojiRenderer != null)
            {
                if (nota == "S+" || nota == "S")
                    emojiRenderer.sprite = emojiCoracao;
                else if (nota == "A" || nota == "B")
                    emojiRenderer.sprite = emojiFeliz;
                else if (nota == "C" || nota == "D")
                    emojiRenderer.sprite = emojiNeutro;
                else
                    emojiRenderer.sprite = emojiBravo;
            }
            yield return new WaitForSecondsRealtime(0.5f);
            Vector2 offsetdino = new Vector2(-7.09f, 4f);
            Vector2 posInicial = meteorofinal.transform.position;
            Vector2 posFinal = consumidor.transform.position;
            float tempoPassado = 0f;

            while (tempoPassado < tempoDeMovimento)
            {
                tempoPassado += Time.unscaledDeltaTime;
                float t = tempoPassado / tempoDeMovimento;
                meteorofinal.transform.position = Vector2.Lerp(posInicial, posFinal + offsetdino, t);
                yield return null;
            }
            meteorofinal.transform.position = posFinal + offsetdino;
            Time.timeScale = 1f;
            SceneManager.LoadScene(cenaFim);
        }
    }
}
