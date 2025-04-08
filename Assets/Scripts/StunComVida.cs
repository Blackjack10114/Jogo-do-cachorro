using UnityEngine;
using System.Collections;

public class StunControllerComVida : MonoBehaviour
{
    public int acertosParaStun = 3;
    private int acertosTomados = 0;

    public float duracaoStun = 3f;

    private bool estaAtordoado = false;
    private PlayerMov playerMov;
    private Rigidbody2D rb;

    void Start()
    {
        playerMov = GetComponent<PlayerMov>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TomarDano(float dano)
    {
        if (estaAtordoado)
        {
            Debug.Log("Jogador já está atordoado, ignorando dano.");
            return;
        }

        acertosTomados++;
        int faltam = acertosParaStun - acertosTomados;

        Debug.Log($"Jogador tomou dano! ({acertosTomados}/{acertosParaStun}) Faltam {Mathf.Max(faltam, 0)} para ficar atordoado.");

        if (acertosTomados >= acertosParaStun)
        {
            StartCoroutine(Atordoar());
        }
    }

    private IEnumerator Atordoar()
    {
        estaAtordoado = true;

        Debug.Log("JOGADOR ATORDOADO!");

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (playerMov != null)
            playerMov.HabilitarMovimento(false);

        StartCoroutine(PiscarDuranteStun());

        yield return new WaitForSeconds(duracaoStun);

        estaAtordoado = false;
        acertosTomados = 0;

        if (playerMov != null)
            playerMov.HabilitarMovimento(true);

        Debug.Log("Jogador se recuperou do atordoamento.");
    }

    private IEnumerator PiscarDuranteStun()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogWarning("SpriteRenderer não encontrado!");
            yield break;
        }

        float tempo = 0f;

        while (tempo < duracaoStun)
        {
            sr.color = Color.red; // Cor visível para mostrar que está atordoado
            yield return new WaitForSeconds(0.2f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.2f);
            tempo += 0.4f;
        }
    }


    public bool EstaAtordoado() => estaAtordoado;
}
