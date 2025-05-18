using UnityEngine;
using UnityEngine.UI;

public class Stamina_Barra : MonoBehaviour
{
    private GameObject Player, fundostamina;
    private Color CorMetade, CorFinal, Cormaismetade;
    public Image Barrastamina;
    public float Stamina;
    private float StaminaMax = 100;
    private PlayerMov VerStamina;
    private Vector3 offset = new Vector3(0f,4.5f,0f);
    private SpriteRenderer SrFundo;
    private Image SrBarra;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        VerStamina = Player.GetComponent<PlayerMov>();
        fundostamina = GameObject.Find("Fundo_Stamina");
        SrFundo = fundostamina.GetComponent<SpriteRenderer>();
        SrBarra = this.GetComponent<Image>();
        CorMetade = new Color (255 / 255f, 200 / 255f, 37 / 255f, 1f);
        CorFinal = new Color (202 / 255f, 17 / 255f, 46 / 255f, 1f);
        Cormaismetade = new Color (66 / 255f, 204 / 255f, 69 / 255f, 1f);
    }

    void Update()
    {
        Stamina = VerStamina.stamina;
        if (Stamina < 0)
        {
            Stamina = 0;
        }
        verificarcor();
        VerificarVisivel();
        Barrastamina.fillAmount = Stamina / StaminaMax;
        transform.position = Player.transform.position + offset;
    }
    private void verificarcor()
    {
        if (VerStamina.stamina <= StaminaMax / 2)
        {
            Barrastamina.color = CorMetade;
        }
        if (VerStamina.stamina <= StaminaMax / 4)
        {
            Barrastamina.color = CorFinal;
        }
        if (VerStamina.stamina > StaminaMax / 2)
        {
            Barrastamina.color = Cormaismetade;
        }
    }
    private void VerificarVisivel()
    {
        if (VerStamina.stamina >= 100)
        {
            SrFundo.enabled = false;
            SrBarra.enabled = false;
        }
        else
        {
            SrFundo.enabled = true;
            SrBarra.enabled = true;
        }
    }
}
