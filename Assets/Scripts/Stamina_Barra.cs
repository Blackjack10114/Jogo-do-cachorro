using UnityEngine;
using UnityEngine.UI;

public class Stamina_Barra : MonoBehaviour
{
    private GameObject Player, fundostamina;
    public Color CorMetade, CorFinal, cormaismetade;
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
        CorMetade.a = 1f;
        CorFinal.a = 1f;
        cormaismetade.a = 1f;
        fundostamina = GameObject.Find("Fundo_Stamina");
        SrFundo = fundostamina.GetComponent<SpriteRenderer>();
        SrBarra = this.GetComponent<Image>();
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
        if (VerStamina.stamina >= StaminaMax / 2)
        {
            Barrastamina.color = cormaismetade;
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
