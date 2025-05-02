using UnityEngine;

public class Buraco : MonoBehaviour
{
    public GameObject Checkpoint = null;
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
