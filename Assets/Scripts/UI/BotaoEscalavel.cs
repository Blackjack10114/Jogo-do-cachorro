using UnityEngine;
using UnityEngine.EventSystems;

public class BotaoEscalavel : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private float escalaSelecionado = 1.2f;
    [SerializeField] private float velocidade = 10f;

    private Vector3 escalaOriginal;
    private bool selecionado;

    void Awake()
    {
        escalaOriginal = transform.localScale;
    }

    void Update()
    {
        Vector3 alvo = selecionado ? escalaOriginal * escalaSelecionado : escalaOriginal;
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            alvo,
            Time.unscaledDeltaTime * velocidade
        );
    }

    public void OnSelect(BaseEventData eventData)
    {
        selecionado = true;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        selecionado = false;
    }
}
