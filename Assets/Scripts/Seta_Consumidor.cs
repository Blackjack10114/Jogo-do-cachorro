using UnityEngine;

public class Seta_Consumidor : MonoBehaviour
{
    public Vector3 offset = new Vector3(4f, 0f, 0f);
    private GameObject Consumidor, Player;
    private bool visivel;
    void Start()
    {
        visivel = true;
        Player = GameObject.FindWithTag("Player");
        Consumidor = GameObject.FindWithTag("Consumidor");
    }

    void Update()
    {
        if (visivel)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("invisivel");
                GetComponent<SpriteRenderer>().enabled = false;
                visivel = false;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetComponent<SpriteRenderer>().enabled = true;
                visivel = true;
            }
        }
        transform.position = Player.transform.position + offset;

        Vector3 direction = Consumidor.transform.position - Player.transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
