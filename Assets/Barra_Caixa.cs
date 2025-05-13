using UnityEngine;
using UnityEngine.UI;

public class Barra_Caixa : MonoBehaviour
{
    private GameObject Player;
    public Color CorMetade, CorFinal;
    public Image Barravida;
    public float Vida, VidaMax;
    private Dano Vervida;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Vervida = Player.GetComponent<Dano>();
    }

    void Update()
    {
        Vida = Vervida.pv;
        if (Vida < 0)
        {
            Vida = 0;
        }
        Barravida.fillAmount = Vida / VidaMax;
    }
}
