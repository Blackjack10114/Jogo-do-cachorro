using UnityEngine;
using UnityEngine.InputSystem;

public class Seta_Consumidor : MonoBehaviour
{
    public Vector3 offset = new(4f, 0f, 0f);
    private GameObject Consumidor, Player;
    private bool visivel;

    public InputController inputActions; //Chama a função do input(Controlador)

    public void Awake()
    {
        inputActions = new InputController();
    }
    void Start()
    {
        visivel = true;
        Player = GameObject.FindWithTag("Player");
        Consumidor = GameObject.FindWithTag("Consumidor");
    }

    void Update()
    {
        //Alterna entre a seta 
        if(inputActions.Player.Seta.WasPressedThisFrame())
        { 
           Debug.Log("seta invisivel");
            visivel = !visivel;
            GetComponent<SpriteRenderer>().enabled = visivel;
        }

       //posiciona e rotaciona a seta
        transform.position = Player.transform.position + offset;

        Vector3 direction = Consumidor.transform.position - Player.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    //Caso queira desativar ou ativar o input em algum momento
    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

}
