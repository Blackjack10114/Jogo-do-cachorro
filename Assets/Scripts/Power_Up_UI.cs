using System.Collections;
using UnityEngine;

public class Power_Up_UI : MonoBehaviour
{
    private Power_Up_Coletavel bool_script;
    public GameObject Power_Up;
    public GameObject doguinho;
    private PlayerMov duracao;
    bool turbo_ativado;
    bool gourmet_ativado;
    bool bolha_ativada;
    private Dano Bolha;
    void Start()
    {
        GetComponent<Renderer>().enabled = false;
        bool_script = Power_Up.GetComponent<Power_Up_Coletavel>();
        duracao = doguinho.GetComponent<PlayerMov>();
        Bolha = doguinho.GetComponent<Dano>();
    }
    void Update()
    {
        if (bool_script.PowerUp_coletado == true && bool_script != null)
        {
            GetComponent<Renderer>().enabled = true;
        }
        if (duracao.isTurboActive == true)
        {
            turbo_ativado = true;
        }
        if (turbo_ativado == true)
        {
            if (duracao.isTurboActive == false)
            {
                StartCoroutine(DelayVerificaçãotempo());
            }
        }
        if (duracao.isGourmetActive == true)
        {
            gourmet_ativado = true;
        }
        if (gourmet_ativado == true)
        {
            if (duracao.isGourmetActive == false)
            {
                StartCoroutine(DelayVerificaçãotempo());
            }
        }
        if (Bolha.isInvincible == true)
        {
            bolha_ativada = true;
        }
        if (bolha_ativada == true)
        {
            if (Bolha.isInvincible == false)
            {
                StartCoroutine(DelayVerificaçãotempo());
            }
        }
    }
    
    
    private IEnumerator DelayVerificaçãotempo()
    {
        yield return new WaitForSeconds(0.1f);
        turbo_ativado = false;
        gourmet_ativado = false;
        GetComponent<Renderer>().enabled = false;
    }
}
