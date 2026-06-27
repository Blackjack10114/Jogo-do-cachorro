using UnityEngine;
using UnityEngine.InputSystem;

public class InputIconManager : MonoBehaviour
{
    public enum TipoInput { TecladoMouse, Controle }
    public TipoInput inputAtual;

    [Header("Ícones por ação")]
    [SerializeField] private IconeAcao[] iconesPorAcao;

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            var action = obj as InputAction;
            if (action != null && action.activeControl != null)
            {
                var device = action.activeControl.device;

                if (device is Gamepad && inputAtual != TipoInput.Controle)
                {
                    inputAtual = TipoInput.Controle;
                    TrocarIcones();
                }
                else if ((device is Keyboard || device is Mouse) && inputAtual != TipoInput.TecladoMouse)
                {
                    inputAtual = TipoInput.TecladoMouse;
                    TrocarIcones();
                }
            }
        }
    }

    private void TrocarIcones()
    {
        bool usandoControle = (inputAtual == TipoInput.Controle);

        foreach (var acao in iconesPorAcao)
        {
            foreach (var obj in acao.teclado)
                obj.SetActive(!usandoControle);

            foreach (var obj in acao.controle)
                obj.SetActive(usandoControle);
        }
    }

    [System.Serializable]
    public class IconeAcao
    {
        public string nomeAcao;          // Ex: "Andar", "Correr", "Pular"
        public GameObject[] teclado;     // Ex: A, D, Setas
        public GameObject[] controle;    // Ex: RT, A, RB
    }

}
