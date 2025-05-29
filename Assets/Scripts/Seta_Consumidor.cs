using UnityEngine;

public class Seta_Consumidor : MonoBehaviour
{
    public Vector3 offset = new Vector3(4f, 0f, 0f);
    private GameObject Consumidor, Player;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Consumidor = GameObject.FindWithTag("Consumidor");
    }

    void Update()
    {

        transform.position = Player.transform.position + offset;

        Vector3 direction = Consumidor.transform.position - Player.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
