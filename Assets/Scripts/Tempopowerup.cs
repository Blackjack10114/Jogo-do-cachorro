using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Tempopowerup : MonoBehaviour
{
    private PlayerMov Duration;
    private Up_UI_Teste tempos;
    private GameObject Player, Powerup;
    private Text Texto = null;
    private bool turbo_ativado, gourmet_ativado, duplo_ativado;
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
        if (Duration.temPuloDuplo == true)
        {
            duplo_ativado = true;
        }
        mostrartempo();
        // verificação turbo
        if (turbo_ativado == true)
        {
            if (Duration.turboTimer <= 0 && tempos.turbo_ativado == false) 
            {
                tempos.turbotempo = false;
                turbo_ativado = false;
                StartCoroutine(Delaydestruirturbo());
            }
        }
        // verificação gourmet
        if (gourmet_ativado == true)
        {
            if (Duration.gourmetTimer <= 0 && tempos.gourmet_ativado == false) 
            {
                tempos.gourmettempo = false;
                gourmet_ativado = false;
                StartCoroutine(Delaydestruirgourmet());
            }
        }
        // verificação pulo duplo
        if (duplo_ativado == true)
        {
            if (Duration.duracaoPuloDuploAtual <= 0 && tempos.pulo_duplo_ativado == false)
            {
                tempos.pulotempo = false;
                duplo_ativado = false;
                StartCoroutine(Delaydestruirduplo());
            }
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
        if (duplo_ativado)
        {
            Texto.text = Mathf.Round(Duration.duracaoPuloDuploAtual).ToString();
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
    private IEnumerator Delaydestruirduplo()
    {
        yield return new WaitForSeconds(0.01f);
        duplo_ativado = false;
        Destroy(this.gameObject);
    }
}
