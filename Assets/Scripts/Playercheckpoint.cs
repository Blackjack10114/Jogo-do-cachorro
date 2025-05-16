using UnityEngine;

public class Playercheckpoint : MonoBehaviour
{
    private Vector2 lastCheckpointPosition;

    void Start()
    {
        lastCheckpointPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpointPosition = other.transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Buraco"))
        {
            transform.position = lastCheckpointPosition;
        }
    }
}
