using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFadeUI : MonoBehaviour
{
    public static SceneFadeUI Instance;

    [Header("Configurações")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private PlayerMov playerMov;
    private Jump jump;

    private bool bloqueouInput = false;
    public static bool FadeEmAndamento { get; private set; }

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

        FadeEmAndamento = true;
        canvasGroup.blocksRaycasts = true;  // bloqueia cliques
        canvasGroup.interactable = false;

        // Pega os scripts do jogador (na cena atual)
        playerMov = FindFirstObjectByType<PlayerMov>();
        jump = FindFirstObjectByType<Jump>();

        // === BLOQUEAR INPUTS ===
        if (playerMov != null)
        {
            playerMov.ResetarInput();
            playerMov.PararSomCorrida();
            playerMov.enabled = false;
        }
        if (jump != null)
        {
            jump.PararSomPulo();
            jump.enabled = false;
        }
        bloqueouInput = true;

        // --- FADE OUT (escurece a tela) ---
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f; // garante alpha máximo

        // Carrega a próxima CENA sem ativar ainda
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(cenaDestino);
        asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress < 0.9f)
            yield return null;

        asyncLoad.allowSceneActivation = true;

        yield return null; // espera cena ativar

        // Atualiza referências para a nova cena
        playerMov = FindFirstObjectByType<PlayerMov>();
        jump = FindFirstObjectByType<Jump>();

        // --- FADE IN (revela a cena nova) ---
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        // === DESBLOQUEAR INPUTS ===
        if (bloqueouInput)
        {
            if (playerMov != null) playerMov.enabled = true;
            if (jump != null) jump.enabled = true;
            bloqueouInput = false;
        }

        // === LIBERA INTERAÇÕES UI ===
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = true;
        FadeEmAndamento = false;
    }
}
