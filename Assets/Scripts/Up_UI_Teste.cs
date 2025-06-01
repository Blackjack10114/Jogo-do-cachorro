using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Up_UI_Teste : MonoBehaviour
{
    private PlayerMov duracao;
    private GameObject Player;
    private Vector3 offsetpowerup;

    [HideInInspector] public bool turbo_ativado, tempoturbo, tempogourmet;
    [HideInInspector] public bool gourmettempo, turbotempo, pulotempo;
    [HideInInspector] public bool gourmet_ativado;
    public bool pulo_duplo_ativado;

    private bool turbo_insta, gourmet_insta, pulo_insta, bolha_insta;
    private bool bolha_ativada;

    // Prefab references
    private GameObject TurboPrefabRef;
    private GameObject GourmetPrefabRef;
    private GameObject BolhaPrefabRef;
    private GameObject PuloPrefabRef;
    private Text TempoPrefab;

    // Instances
    private GameObject TurboInstance;
    private GameObject GourmetInstance;
    private GameObject BolhaInstance;
    private GameObject PuloInstance;

    private Dano Bolha;
    private Caixa VerPuloDuplo;

    // UI Timer Texts
    private Text TempoTurbo;
    private Text TempoGourmet;
    private Text TempoPulo;

    void Start()
    {
        offsetpowerup = new Vector3(16f, 0f, 0f);
        Player = GameObject.FindWithTag("Player");
        duracao = Player.GetComponent<PlayerMov>();
        Bolha = Player.GetComponent<Dano>();
        VerPuloDuplo = Player.GetComponent<Caixa>();

        // Load prefabs once
        TurboPrefabRef = Resources.Load<GameObject>("PowerUp_Ração_Turbo_UI");
        GourmetPrefabRef = Resources.Load<GameObject>("PowerUp_Racao_Dourada_UI");
        BolhaPrefabRef = Resources.Load<GameObject>("PowerUp_Bolha_UI");
        PuloPrefabRef = Resources.Load<GameObject>("PowerUp_PuloDuplo_UI");
        TempoPrefab = Resources.Load<Text>("Tempo_Powerup");
    }

    void Update()
    {
        // --- Turbo ---
        if (duracao.isTurboActive && TurboPrefabRef != null && !turbo_insta)
        {
            turbo_ativado = true;
            Vector3 pos = (gourmet_ativado && gourmet_insta) || (pulo_duplo_ativado && pulo_insta)
                            ? this.transform.position + offsetpowerup
                            : this.transform.position;

            TurboInstance = Instantiate(TurboPrefabRef, pos, Quaternion.identity);
            TurboInstance.transform.parent = this.transform;
            turbo_insta = true;
        }
        if (!duracao.isTurboActive && TurboInstance != null)
        {
            Destroy(TurboInstance);
            turbo_ativado = false;
            turbo_insta = false;
        }

        // --- Gourmet ---
        if (duracao.isGourmetActive && GourmetPrefabRef != null && !gourmet_insta)
        {
            gourmet_ativado = true;
            Vector3 pos = (turbo_ativado && turbo_insta) || (pulo_duplo_ativado && pulo_insta)
                            ? this.transform.position + offsetpowerup
                            : this.transform.position;

            GourmetInstance = Instantiate(GourmetPrefabRef, pos, Quaternion.identity);
            GourmetInstance.transform.parent = this.transform;
            gourmet_insta = true;
        }
        if (!duracao.isGourmetActive && GourmetInstance != null)
        {
            Destroy(GourmetInstance);
            gourmet_ativado = false;
            gourmet_insta = false;
        }

        // --- Bolha ---
        if (Bolha.isInvincible && !bolha_insta)
        {
            Vector3 pos = this.transform.position + new Vector3(8f, 0f, 0f);
            BolhaInstance = Instantiate(BolhaPrefabRef, pos, Quaternion.identity);
            BolhaInstance.transform.parent = this.transform;
            bolha_ativada = true;
            bolha_insta = true;
        }
        if (!Bolha.isInvincible && BolhaInstance != null)
        {
            Destroy(BolhaInstance);
            bolha_ativada = false;
            bolha_insta = false;
        }

        // --- Pulo Duplo ---
        if (duracao.temPuloDuplo && PuloPrefabRef != null && !pulo_insta)
        {
            pulo_duplo_ativado = true;
            Debug.Log("Pulo instanciado");

            Vector3 pos = (gourmet_ativado && gourmet_insta) || (turbo_ativado && turbo_insta)
                            ? this.transform.position + offsetpowerup
                            : this.transform.position;

            PuloInstance = Instantiate(PuloPrefabRef, pos, Quaternion.identity);
            PuloInstance.transform.parent = this.transform;
            pulo_insta = true;
        }
        if (!duracao.temPuloDuplo && PuloInstance != null)
        {
            Destroy(PuloInstance);
            pulo_duplo_ativado = false;
            pulo_insta = false;
        }

        // --- Timer Texts ---
        if (TempoPrefab != null && TempoTurbo == null && turbo_ativado)
        {
            turbotempo = true;
            TempoTurbo = InstanciarTempo("turbo");
        }
        if (TempoTurbo != null && !duracao.isTurboActive)
        {
            turbotempo = false;
            Destroy(TempoTurbo.gameObject);
            TempoTurbo = null;
        }

        if (TempoPrefab != null && TempoGourmet == null && gourmet_ativado)
        {
            gourmettempo = true;
            TempoGourmet = InstanciarTempo("gourmet");
        }
        if (TempoGourmet != null && !duracao.isGourmetActive)
        {
            gourmettempo = false;
            Destroy(TempoGourmet.gameObject);
            TempoGourmet = null;
        }

        if (TempoPrefab != null && TempoPulo == null && pulo_duplo_ativado)
        {
            pulotempo = true;
            TempoPulo = InstanciarTempo("pulo");
        }
        if (TempoPulo != null && !duracao.temPuloDuplo)
        {
            pulotempo = false;
            Destroy(TempoPulo.gameObject);
            TempoPulo = null;
        }
    }

    private Text InstanciarTempo(string tipo)
    {
        Vector3 offsettext = new Vector3(0, -10, 0);
        Vector3 posicaoTexto = Vector3.zero;

        switch (tipo)
        {
            case "turbo":
                posicaoTexto = TurboInstance != null ? TurboInstance.transform.position + offsettext : Vector3.zero;
                break;
            case "pulo":
                posicaoTexto = PuloInstance != null ? PuloInstance.transform.position + offsettext : Vector3.zero;
                break;
            case "gourmet":
                posicaoTexto = GourmetInstance != null ? GourmetInstance.transform.position + offsettext : Vector3.zero;
                break;
        }

        Text novoTexto = Instantiate(TempoPrefab, posicaoTexto, Quaternion.identity);
        novoTexto.transform.SetParent(this.transform);
        return novoTexto;
    }
}
