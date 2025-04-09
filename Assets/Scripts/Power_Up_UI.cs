using UnityEngine;

public class Power_Up_UI : MonoBehaviour
{
    private Power_Up_Coletavel bool_script;
    public GameObject Power_Up;
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        bool_script = Power_Up.GetComponent<Power_Up_Coletavel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bool_script.PowerUp_coletado == true && bool_script != null)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }
}
