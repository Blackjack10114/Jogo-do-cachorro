using UnityEngine;
using UnityEngine.Rendering;

public class coletavel : MonoBehaviour
{
    public bool osso_coletado = false;
    private GameObject Player = null;
    private GameObject Ossinho = null;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Ossinho = GameObject.FindGameObjectWithTag("Osso");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            osso_coletado = true;
            Debug.Log("osso coletado!");
            GetComponent<Renderer>().enabled = false;
            Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Ossinho.GetComponent<Collider2D>());
        }
    }
}
