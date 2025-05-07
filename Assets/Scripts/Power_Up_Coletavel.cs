using System.Security;
using UnityEngine;
using System.Collections;

public class Power_Up_Coletavel : MonoBehaviour
{
    public bool PowerUp_coletado;
    private GameObject Player = null;
    public GameObject Power_up;
    private PlayerMov duracao;
    public GameObject doguinho;
    private Dano Bolha;
    bool turbo_ativado;
    bool gourmet_ativado;
    bool bolha_ativada;
    bool pulo_duplo_ativado;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        duracao = doguinho.GetComponent<PlayerMov>();
        Bolha = doguinho.GetComponent<Dano>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PowerUp_coletado = true;
            GetComponent<Renderer>().enabled = false;
            Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Power_up.GetComponent<Collider2D>());
        }
    }
    private void Update()
    {
        //verificação do turbo
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
        //verificação do gourmet
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
        //verificação da bolha protetora
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
        //verificação do pulo duplo
        if (duracao.temPuloDuplo == true)
        {
            pulo_duplo_ativado = true;
        }
        if (pulo_duplo_ativado == true)
        {
            if (duracao.temPuloDuplo == false)
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
        pulo_duplo_ativado = false;
        PowerUp_coletado = false;
    }
}
