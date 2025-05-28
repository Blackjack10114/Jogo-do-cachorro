using UnityEngine;

public class Seta_Consumidor : MonoBehaviour
{
    Vector3 Pos_seta, Pos_consumidor, Calculo_Pos;
    public Vector3 offset = new Vector3(4f, 0f, 0f);
    GameObject Consumidor, Player;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Consumidor = GameObject.FindWithTag("Consumidor");
    }

    void Update()
    {
        transform.position = Player.transform.position + offset;
        Pos_seta = Player.transform.position;
        Pos_consumidor = Consumidor.transform.position;
        Calculo_Pos = Pos_consumidor - Pos_seta;
        if (Calculo_Pos.x > 0 && Calculo_Pos.y <= 50 && Calculo_Pos.y >= -50)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (Calculo_Pos.x < 0 && Calculo_Pos.y <= 50 && Calculo_Pos.y >= -50)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        if (Calculo_Pos.y >= 50)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        if (Calculo_Pos.y <= -50)
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
    }
}
