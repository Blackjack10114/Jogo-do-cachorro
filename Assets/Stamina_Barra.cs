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
        // não troca a cor ainda
        if (VerStamina.stamina <= VerStamina.stamina / 2) 
        {
            Barrastamina.color = CorMetade;
        }
        if (VerStamina.stamina <= VerStamina.stamina / 4) 
        { 
            Barrastamina.color = CorFinal; 
        }
        Barrastamina.fillAmount = Stamina / StaminaMax;
    }
}
