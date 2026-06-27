using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelecionarAoPassarMouse : MonoBehaviour, IPointerEnterHandler
{
    private Selectable selectable;

    void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (selectable != null && selectable.interactable)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
