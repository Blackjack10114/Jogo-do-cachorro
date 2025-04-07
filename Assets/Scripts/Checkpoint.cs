using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
      // só usar se for fazer o checkpoint de forma melhor
        if (other.CompareTag("Player"))
        {
            Debug.Log("pegou checkpoint");
        }
    }
}
