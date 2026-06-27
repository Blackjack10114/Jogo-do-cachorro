using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderEscalavel : MonoBehaviour,
    ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    [Header("Referências")]
    [SerializeField] private RectTransform handle;

    [Header("Escala")]
    [SerializeField] private float escalaSelecionado = 1.2f;
    [SerializeField] private float velocidade = 10f;

    private Vector3 escalaOriginal;
    private bool selecionado;
    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
        escalaOriginal = handle.localScale;
    }

    void Update()
    {
        if (handle == null) return;

        Vector3 alvo = selecionado
            ? escalaOriginal * escalaSelecionado
            : escalaOriginal;

        handle.localScale = Vector3.Lerp(
            handle.localScale,
            alvo,
            Time.unscaledDeltaTime * velocidade
        );
    }

    // teclado / controle
    public void OnSelect(BaseEventData eventData)
    {
        selecionado = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selecionado = false;
    }

    // mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slider != null && slider.interactable)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}
