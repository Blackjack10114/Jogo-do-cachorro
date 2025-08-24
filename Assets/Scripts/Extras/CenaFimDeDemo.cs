using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CenaFimDeDemoController : MonoBehaviour
{
    public GameObject mensagemAperteBotao; // Texto "Aperte qualquer botão"
    private CanvasGroup mensagemCanvas;    // Controle de transparência para piscar

    private bool podeSair = false;

    void Start()
    {
        // Esconde a mensagem no início
        mensagemAperteBotao.SetActive(false);

        // Pega (ou adiciona) CanvasGroup para controlar o alpha (transparência)
        mensagemCanvas = mensagemAperteBotao.GetComponent<CanvasGroup>();
        if (mensagemCanvas == null)
        {
            mensagemCanvas = mensagemAperteBotao.AddComponent<CanvasGroup>();
        }

        // Começa a rotina de espera
        StartCoroutine(EsperarAntesDeMostrarMensagem());
    }

    IEnumerator EsperarAntesDeMostrarMensagem()
    {
        // Espera 3 segundos garantidos
        yield return new WaitForSeconds(3f);

        // Agora mostra a mensagem
        mensagemAperteBotao.SetActive(true);

        // Inicia o piscar da mensagem
        StartCoroutine(PiscarMensagem());

        // Libera para sair
        podeSair = true;
    }

    IEnumerator PiscarMensagem()
    {
        // Loop até trocar de cena
        while (true)
        {
            // fade out
            for (float t = 0; t < 1.35f; t += Time.deltaTime)
            {
                mensagemCanvas.alpha = Mathf.Lerp(1f, 0f, t);
                yield return null;
            }
            // fade in
            for (float t = 0; t < 1.35f; t += Time.deltaTime)
            {
                mensagemCanvas.alpha = Mathf.Lerp(0f, 1f, t);
                yield return null;
            }
        }
    }

    void Update()
    {
        if (podeSair && Input.anyKeyDown)
        {
            // Faz o fade e troca para o MenuPrincipal
            StartCoroutine(SairParaMenu());
        }
    }

    IEnumerator SairParaMenu()
    {
        // Executa o fade
        yield return new WaitForSeconds(1f);
        SceneFadeUI.Instance.FadeToScene("MenuPrincipal");
    }
}
