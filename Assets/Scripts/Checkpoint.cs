using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector2 pointupdate; // local variable to store position of the current game object

        if (other.tag == "Player") // filter the objects that collide with the checkpoint. You can assign the tag in the inspector
        {

        }
    }
}
