using UnityEngine;
using System.Collections;

public class StunControllerComVida : MonoBehaviour
{
    public int acertosParaStun = 3;
    private int acertosTomados = 0;

    public float duracaoStun = 3f;

    private bool estaAtordoado = false;
    private PlayerMov playerMov;
    private Rigidbody2D rb; // <- Novo

    void Start()
    {
        playerMov = GetComponent<PlayerMov>();
        rb = GetComponent<Rigidbody2D>(); // <- Pegando o Rigidbody
    }

    public void TomarDano(float dano)
    {
        if (estaAtordoado) return;

        acertosTomados++;

        if (acertosTomados >= acertosParaStun)
        {
            StartCoroutine(Atordoar());
        }
    }

    private IEnumerator Atordoar()
    {
        estaAtordoado = true;

        if (playerMov != null)
            playerMov.enabled = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero; // <- Zera a velocidade na hora do stun

        Debug.Log("Atordoado!");

        yield return new WaitForSeconds(duracaoStun);

        estaAtordoado = false;
        acertosTomados = 0;

        if (playerMov != null)
            playerMov.enabled = true;

        Debug.Log("Recuperado!");
    }

    public bool EstaAtordoado() => estaAtordoado;
}
