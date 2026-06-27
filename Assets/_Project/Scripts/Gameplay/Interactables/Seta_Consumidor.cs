using UnityEngine;
using UnityEngine.InputSystem;

public class SetaDirecional : MonoBehaviour
{
    public Vector3 offset = new(4f, 0f, 0f);
    private GameObject consumidor, player;
    private GameObject caixaAtual; // referência da caixa caída
    private bool visivel;

    public InputController inputActions; // Chama a função do input(Controlador)

    public void Awake()
    {
        inputActions = new InputController();
    }

    void Start()
    {
        visivel = true;
        player = GameObject.FindWithTag("Player");
        consumidor = GameObject.FindWithTag("Consumidor");
    }

    void Update()
    {
        // Alterna entre mostrar/esconder a seta
        if (inputActions.Player.Seta.WasPressedThisFrame())
        {
            visivel = !visivel;
            GetComponent<SpriteRenderer>().enabled = visivel;
        }

        if (!visivel) return;

        /* Define o alvo da seta:
         Se existe caixa caída - seta aponta para ela
         Caso contrário seta aponta para o consumidor*/
        GameObject alvo = caixaAtual != null ? caixaAtual : consumidor;

        // Posiciona a seta em relação ao player
        transform.position = player.transform.position + offset;

        // Calcula direção até o alvo
        if (alvo != null)
        {
            Vector3 direction = alvo.transform.position - player.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    // Método chamado pela caixa quando ela "cair"
    public void DefinirCaixaCaida(GameObject caixa)
    {
        caixaAtual = caixa;
    }

    // Método chamado quando o player recuperar a caixa
    public void LimparCaixaCaida()
    {
        caixaAtual = null;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }
}
