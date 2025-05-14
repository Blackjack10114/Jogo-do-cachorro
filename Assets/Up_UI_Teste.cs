using UnityEngine;
using System.Collections;

public class Up_UI_Teste : MonoBehaviour
{
    private PlayerMov duracao;
    private GameObject Player;
    bool turbo_ativado; 
    bool turbo_insta, gourmet_insta, pulo_insta, bolha_insta;
    bool gourmet_ativado;
    bool bolha_ativada;
    bool pulo_duplo_ativado;
    private GameObject TurboPrefab = null;
    private GameObject GourmetPrefab = null;
    private GameObject BolhaPrefab = null;
    private GameObject PuloPrefab = null;
    private Dano Bolha;
    private GameObject objetoreferencia;
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        duracao = Player.GetComponent<PlayerMov>();
        TurboPrefab = Resources.Load<GameObject>("PowerUp_Ração_Turbo_UI");
        GourmetPrefab = Resources.Load<GameObject>("PowerUp_Racao_Dourada_UI");
        BolhaPrefab = Resources.Load<GameObject>("PowerUp_Bolha_UI");
        PuloPrefab = Resources.Load<GameObject>("PowerUp_PuloDuplo_UI");
        Bolha = Player.GetComponent<Dano>();
        objetoreferencia = this.gameObject;
    }
    private void Update()
    {
       //verificação turbo
        if (duracao.isTurboActive == true && TurboPrefab != null && !turbo_insta)
        {
            turbo_ativado = true;
            TurboPrefab = Instantiate(TurboPrefab, this.transform.position, Quaternion.identity);
            TurboPrefab.transform.parent = this.transform;
            turbo_insta = true;
        }
        if (turbo_ativado == true)
        {
            if (duracao.isTurboActive == false)
            {
                Destroy(TurboPrefab);
                turbo_ativado = false;
                turbo_insta = false;
            }
        }
        //Verificaçao gourmet
        if (duracao.isGourmetActive == true && GourmetPrefab != null && !gourmet_insta)
        {
            gourmet_ativado = true;
            GourmetPrefab = Instantiate(GourmetPrefab, this.transform.position, Quaternion.identity);
            GourmetPrefab.transform.parent = this.transform;
            gourmet_insta = true;
        }
        if (gourmet_ativado == true)
        {
            if (duracao.isGourmetActive == false)
            {
                Destroy(GourmetPrefab);
                gourmet_ativado = false;
                gourmet_insta = false;
            }
        }
        // verificação bolha
        if (Bolha.isInvincible == true && !bolha_insta && BolhaPrefab != null)
        {
            bolha_ativada = true;
            Vector3 offset = new Vector3(-8f, 0f, 0f);
            Vector3 spawnPosition = objetoreferencia.transform.position + offset;
            BolhaPrefab = Instantiate(BolhaPrefab, spawnPosition, Quaternion.identity);
            BolhaPrefab.transform.parent = this.transform;
            bolha_insta = true;
        }
        if (bolha_ativada == true)
        {
            if (Bolha.isInvincible == false)
            {
                Destroy(BolhaPrefab);
                bolha_ativada = false;
                bolha_insta = false;
            }
        }
        // verificação pulo duplo
        if (duracao.temPuloDuplo == true && PuloPrefab != null && !pulo_insta)
        {
            pulo_duplo_ativado = true;
            PuloPrefab = Instantiate(PuloPrefab, this.transform.position, Quaternion.identity);
            PuloPrefab.transform.parent = this.transform;
            pulo_insta = true;
        }
        if (pulo_duplo_ativado == true)
        {
            if (duracao.temPuloDuplo == false)
            {
                Destroy(PuloPrefab);
                pulo_duplo_ativado = false;
                pulo_insta = false;
            }
        }
    }
}
