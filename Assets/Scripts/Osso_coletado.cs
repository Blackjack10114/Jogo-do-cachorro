using UnityEngine;

public class Osso_coletado : MonoBehaviour
{
    private coletavel bool_script;
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bool_script.osso_coletado == true)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }
}
