using UnityEngine;

public class Power_Up_UI : MonoBehaviour
{
    private Power_Up_Coletavel bool_script;
    public GameObject Power_Up;
    private PowerUp duracao;
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        bool_script = Power_Up.GetComponent<Power_Up_Coletavel>();
        duracao = Power_Up.GetComponent<PowerUp>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bool_script.PowerUp_coletado == true && bool_script != null)
        {
            GetComponent<Renderer>().enabled = true;
        }
        if (duracao.duration <= 0)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }
}
