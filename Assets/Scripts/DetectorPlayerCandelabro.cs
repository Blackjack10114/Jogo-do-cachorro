using UnityEngine;

public class DetectorPlayerCandelabro : MonoBehaviour
{
    private Candelabro candelabro;

    void Start()
    {
        candelabro = GetComponentInParent<Candelabro>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            candelabro.DetectouPlayer();
        }
    }
}
