using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFadeUI : MonoBehaviour
{
    public static SceneFadeUI Instance;

    [Header("Configurações")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        canvasGroup.alpha = 0f; // começa transparente
    }

    public void FadeToScene(string cenaDestino)
    {
        StartCoroutine(FadeAndSwitchScenes(cenaDestino));
    }

    private IEnumerator FadeAndSwitchScenes(string cenaDestino)
    {
        // FADE OUT (escurece a tela)
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // garante alpha máximo no final até trocar a cena

        // Carrega a próxima CENA sem ativar ainda
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(cenaDestino);
        asyncLoad.allowSceneActivation = false;

        // Espera carregar até 90%
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        // Ativa a cena carregada
        asyncLoad.allowSceneActivation = true;

        // Espera o próximo frame para cena estar visível
        yield return null;

        // FADE IN (revela a nova cena)
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f; // totalmente transparente no fim
    }
}
