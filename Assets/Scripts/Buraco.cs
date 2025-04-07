using UnityEngine;

public class Buraco : MonoBehaviour
{
    private GameObject Player = null;
    public GameObject Checkpoint = null;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        GetComponent<Renderer>().enabled = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.transform.position = Checkpoint.transform.position;
        }
    }
}
