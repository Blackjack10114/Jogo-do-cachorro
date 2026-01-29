using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoScrollView : MonoBehaviour
{
    [Header("Referências")]
    public ScrollRect scrollRect;

    [Header("Configuração")]
    public float smoothSpeed = 10f;

    private bool autoScrollAtivo = true;
    private bool ignorarPrimeiroFrame;

    void OnEnable()
    {
        ignorarPrimeiroFrame = true;
        autoScrollAtivo = true; 
    }

    void Update()
    {
        if (scrollRect == null || scrollRect.content == null || scrollRect.viewport == null)
            return;

        if (ignorarPrimeiroFrame)
        {
            ignorarPrimeiroFrame = false;
            return;
        }

        DetectarInput();

        if (!autoScrollAtivo)
            return;

        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null || !current.transform.IsChildOf(scrollRect.content))
            return;

        RectTransform content = scrollRect.content;
        RectTransform selected = current.GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport;

        float contentHeight = content.rect.height;
        float viewportHeight = viewport.rect.height;

        if (contentHeight <= viewportHeight)
            return;

        // primeiro item - força topo
        if (IsPrimeiroItemAtivo(selected, content))
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition,
                1f,
                Time.unscaledDeltaTime * smoothSpeed
            );
            return;
        }

        // último item - força fundo
        if (IsUltimoItemAtivo(selected, content))
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition,
                0f,
                Time.unscaledDeltaTime * smoothSpeed
            );
            return;
        }

        // centraliza o item selecionado
        float itemPosY = Mathf.Abs(selected.localPosition.y);
        float itemHeight = selected.rect.height;

        float target =
            1f - ((itemPosY - viewportHeight * 0.5f + itemHeight * 0.5f)
            / (contentHeight - viewportHeight));

        target = Mathf.Clamp01(target);

        scrollRect.verticalNormalizedPosition = Mathf.Lerp(
            scrollRect.verticalNormalizedPosition,
            target,
            Time.unscaledDeltaTime * smoothSpeed
        );
    }

    private void DetectarInput()
    {
        // mouse mexeu no scroll
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0.01f || Input.GetMouseButton(0))
        {
            autoScrollAtivo = false;
            return;
        }

        // teclado / controle
        if (Input.anyKeyDown)
        {
            autoScrollAtivo = true;
        }
    }

    bool IsPrimeiroItemAtivo(RectTransform selected, RectTransform content)
    {
        foreach (Transform child in content)
        {
            if (!child.gameObject.activeSelf)
                continue;

            return child == selected;
        }
        return false;
    }

    bool IsUltimoItemAtivo(RectTransform selected, RectTransform content)
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            if (!content.GetChild(i).gameObject.activeSelf)
                continue;

            return content.GetChild(i) == selected;
        }
        return false;
    }
}
