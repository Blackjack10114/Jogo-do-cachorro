using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class vida : MonoBehaviour
{
    public Text counterText;
    public GameObject Sprite_Dog_Caixa_Normal_0;
    private Dano bool_script;
    public void Start()
    {
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<Dano>();
    }

    public void Update()
    {
        counterText.text = Mathf.Round(bool_script.pv).ToString();
    }
}