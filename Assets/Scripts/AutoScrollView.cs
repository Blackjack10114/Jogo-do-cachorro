using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AutoScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public float smoothSpeed = 10f;
    public float padding = 10f; // espaço extra em pixels
    public float disableTime = 1f; // tempo que o auto scroll fica desativado após mexer no scroll

    private float disableTimer;

    void Update()
    {
        //  Se mexeu no scroll manualmente -> pausa auto scroll
        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0.01f || Input.GetMouseButton(0))
        {
            disableTimer = disableTime;
        }

        if (disableTimer > 0)
        {
            disableTimer -= Time.unscaledDeltaTime;
            return; // não roda autoscroll enquanto o usuário tá controlando
        }

        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == null || !current.transform.IsChildOf(scrollRect.content))
            return;

        RectTransform content = scrollRect.content;
        RectTransform selected = current.GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport;

        float contentHeight = CalculateContentHeight(content);
        float viewportHeight = viewport.rect.height;

        Vector3[] viewportCorners = new Vector3[4];
        Vector3[] selectedCorners = new Vector3[4];

        viewport.GetWorldCorners(viewportCorners);
        selected.GetWorldCorners(selectedCorners);

        float viewportTop = viewportCorners[1].y;
        float viewportBottom = viewportCorners[0].y;

        float selectedTop = selectedCorners[1].y + padding;
        float selectedBottom = selectedCorners[0].y - padding;

        float diff = 0f;

        if (selectedTop > viewportTop) // fora acima
        {
            diff = selectedTop - viewportTop;
        }
        else if (selectedBottom < viewportBottom) // fora abaixo
        {
            diff = selectedBottom - viewportBottom;
        }
        else
        {
            return; // já visível
        }

        float normalizedDiff = diff / (contentHeight - viewportHeight);
        float currentNorm = scrollRect.verticalNormalizedPosition;

        float targetNorm = Mathf.Clamp01(currentNorm + normalizedDiff);

        scrollRect.verticalNormalizedPosition =
            Mathf.Lerp(currentNorm, targetNorm, Time.unscaledDeltaTime * smoothSpeed);
    }

    private float CalculateContentHeight(RectTransform content)
    {
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        foreach (RectTransform child in content)
        {
            if (!child.gameObject.activeSelf) continue;

            Vector3[] corners = new Vector3[4];
            child.GetWorldCorners(corners);

            minY = Mathf.Min(minY, corners[0].y);
            maxY = Mathf.Max(maxY, corners[1].y);
        }

        return Mathf.Abs(maxY - minY);
    }
}
