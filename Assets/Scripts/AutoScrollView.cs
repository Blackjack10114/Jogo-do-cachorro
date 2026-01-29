using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float smoothSpeed = 10f;
    public float disableTime = 0.5f;

    private float disableTimer;
    private bool ignorarPrimeiroFrame;

    void OnEnable()
    {
        ignorarPrimeiroFrame = true;
        disableTimer = disableTime;
    }

    void Update()
    {
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0.01f)
        {
            disableTimer = disableTime;
            return;
        }

        if (scrollRect == null || scrollRect.content == null || scrollRect.viewport == null)
            return;

        if (ignorarPrimeiroFrame)
        {
            ignorarPrimeiroFrame = false;
            return;
        }

        if (disableTimer > 0f)
        {
            disableTimer -= Time.unscaledDeltaTime;
            return;
        }

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

        // posição Y do item dentro do content
        float itemPosY = Mathf.Abs(selected.localPosition.y);
        float itemHeight = selected.rect.height;

        // se for o primeiro item ativo -> força topo
        if (IsPrimeiroItemAtivo(selected, content))
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition,
                1f,
                Time.unscaledDeltaTime * smoothSpeed
            );
            return;
        }

        // se for o último item ativo -> força fundo
        if (IsUltimoItemAtivo(selected, content))
        {
            scrollRect.verticalNormalizedPosition = Mathf.Lerp(
                scrollRect.verticalNormalizedPosition,
                0f,
                Time.unscaledDeltaTime * smoothSpeed
            );
            return;
        }

        // posiciona o scroll baseado no item
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
