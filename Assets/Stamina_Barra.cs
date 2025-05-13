using UnityEngine;
using UnityEngine.UI;

public class Stamina_Barra : MonoBehaviour
{
    private GameObject Player;
    public Color CorMetade, CorFinal;
    public Image Barrastamina;
    public float Stamina, StaminaMax;
    private PlayerMov VerStamina;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        VerStamina = Player.GetComponent<PlayerMov>();
    }

    void Update()
    {
        Stamina = VerStamina.stamina;
        if (Stamina < 0)
        {
            Stamina = 0;
        }
        Barrastamina.fillAmount = Stamina / StaminaMax;
        Debug.Log(VerStamina.stamina);
    }
}
