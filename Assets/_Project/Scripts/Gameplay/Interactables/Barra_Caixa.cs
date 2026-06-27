using UnityEngine;
using UnityEngine.UI;

public class Barra_Caixa : MonoBehaviour
{
    private GameObject Player;
    public Color CorMuitavida, CorPoucavida;
    public Image Barravida;
    public float Vida, VidaMax;
    private Dano Vervida;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Vervida = Player.GetComponent<Dano>();
        CorPoucavida = new Color(202 / 255f, 17 / 255f, 46 / 255f, 1f);
        CorMuitavida = new Color(255 / 255f, 200 / 255f, 37 / 255f, 1f);
    }

    void Update()
    {
        Vida = Vervida.pv;
        if (Vida < 0)
        {
            Vida = 0;
        }
        Barravida.fillAmount = Vida / VidaMax;
        verificarcorvida();
    }
    private void verificarcorvida()
    {
        if (Vervida.pv <= 40)
        {
            Barravida.color = CorPoucavida;
        }
        else
        {
            Barravida.color = CorMuitavida;
        }
    }
}
