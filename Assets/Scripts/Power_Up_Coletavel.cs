using UnityEngine;

public class Power_Up_Coletavel : MonoBehaviour
{
    public bool PowerUp_coletado = false;
    private GameObject Player = null;
    public GameObject Power_up;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PowerUp_coletado = true;
            GetComponent<Renderer>().enabled = false;
            Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Power_up.GetComponent<Collider2D>());
        }

    }
}
