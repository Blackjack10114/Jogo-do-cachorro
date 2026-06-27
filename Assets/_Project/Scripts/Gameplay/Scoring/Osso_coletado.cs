using UnityEngine;

public class Osso_coletado : MonoBehaviour
{
    private coletavel bool_script;
    public GameObject Ossinho;
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        bool_script = Ossinho.GetComponent<coletavel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bool_script.osso_coletado == true && bool_script != null)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }
}
