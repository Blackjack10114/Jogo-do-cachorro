using UnityEngine;
using UnityEngine.EventSystems;

public class PanelFocusManager : MonoBehaviour
{
    [SerializeField] private GameObject defaultButton;

    private GameObject lastSelected;

    public void OnOpen()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (lastSelected != null && lastSelected.activeInHierarchy)
        {
            EventSystem.current.SetSelectedGameObject(lastSelected);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }

    public void OnClose()
    {
        lastSelected = EventSystem.current.currentSelectedGameObject;
    }
}
