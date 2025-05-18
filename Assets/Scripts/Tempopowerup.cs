using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tempopowerup : MonoBehaviour
{
    private PlayerMov Duration;
    private Up_UI_Teste tempos;
    private GameObject Player, Powerup;
    private Text Texto = null;
    private bool turbo_ativado, gourmet_ativado;
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Duration = Player.GetComponent<PlayerMov>();
        Texto = GetComponent<Text>();
        Powerup = GameObject.FindWithTag("PowerUpUi");
        tempos = Powerup.GetComponent<Up_UI_Teste>();
    }
    private void Update()
    {
        if (Duration.isTurboActive == true)
        {
            turbo_ativado = true;
        }
        if (Duration.isGourmetActive == true)
        {
            gourmet_ativado = true;
        }
        mostrartempo();
        if (Duration.turboTimer <= 0 && turbo_ativado == true)
        {
            tempos.turbotempo = false;
            turbo_ativado = false;
            StartCoroutine(Delaydestruirturbo());
        }
        if (Duration.gourmetTimer <= 0 && gourmet_ativado == true)
        {
            tempos.gourmettempo = false;
            gourmet_ativado = false;
            StartCoroutine(Delaydestruirgourmet());
        }
    }
    private void mostrartempo()
    {
        if (turbo_ativado)
        {
            Texto.text = Mathf.Round(Duration.turboTimer).ToString();
        }
        if (gourmet_ativado)
        {
            Texto.text = Mathf.Round(Duration.gourmetTimer).ToString();
        }
    }
    private IEnumerator Delaydestruirturbo()
    {
        yield return new WaitForSeconds(0.01f);
        turbo_ativado = false;
        Destroy(this.gameObject);
    }
    private IEnumerator Delaydestruirgourmet()
    {
        yield return new WaitForSeconds(0.01f);
        gourmet_ativado = false;
        Destroy(this.gameObject);
    }
}
