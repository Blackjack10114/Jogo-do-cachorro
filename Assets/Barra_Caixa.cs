using UnityEngine;
using UnityEngine.UI;

public class Barra_Caixa : MonoBehaviour
{
    private GameObject Player;
    public Color CorMetade, CorFinal;
    public Image Barravida;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Barravida.fillAmount = Player.Oxigenio / Player.MaxOxigen;
    }
}
