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
    bool turbo_ativado;
    bool gourmet_ativado;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        duracao = doguinho.GetComponent<PlayerMov>();
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
    }
    private IEnumerator DelayVerificaçãotempo()
    {
        yield return new WaitForSeconds(0.1f);
        turbo_ativado = false;
        gourmet_ativado = false;
        PowerUp_coletado = false;
        GetComponent<Renderer>().enabled = false;
    }
}
