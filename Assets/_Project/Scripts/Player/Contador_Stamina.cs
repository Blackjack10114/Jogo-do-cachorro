using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Script : MonoBehaviour
{
    public Text counterText;
    public GameObject Sprite_Dog_Caixa_Normal_0;
    private PlayerMov bool_script;
    public void Start()
    {
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<PlayerMov>();
    }

    public void Update()
    {
        counterText.text = Mathf.Round(bool_script.stamina).ToString();
    }
    }
