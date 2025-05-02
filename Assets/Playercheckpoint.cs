using UnityEngine;

public class Playercheckpoint : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    void Start()
    {
        // Define a posição inicial como primeiro checkpoint
        lastCheckpointPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            // Atualiza o último checkpoint
            lastCheckpointPosition = other.transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Buraco"))
        {
            // Teleporta o jogador de volta ao checkpoint
            transform.position = lastCheckpointPosition;
        }
    }
}
