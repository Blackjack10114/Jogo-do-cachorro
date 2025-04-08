using UnityEngine;
using System.Collections;

public class StunControllerComVida : MonoBehaviour
{
    public float vidaMaxima = 30f;
    public float vidaAtual;

    public float duracaoStun = 3f;
    public float tempoRecuperacaoVida = 5f; // quanto tempo para recuperar vida se não tomar dano
    public float velocidadeRecuperacao = 5f;

    private bool estaAtordoado = false;
    private PlayerMov playerMov;
    private float tempoDesdeUltimoDano = 0f;

    void Start()
    {
        vidaAtual = vidaMaxima;
        playerMov = GetComponent<PlayerMov>();
    }

    void Update()
    {
        if (!estaAtordoado)
        {
            tempoDesdeUltimoDano += Time.deltaTime;

            if (tempoDesdeUltimoDano >= tempoRecuperacaoVida && vidaAtual < vidaMaxima)
            {
                vidaAtual += velocidadeRecuperacao * Time.deltaTime;
                vidaAtual = Mathf.Min(vidaAtual, vidaMaxima);
            }
        }
    }

    public void TomarDano(float dano)
    {
        if (estaAtordoado) return;

        tempoDesdeUltimoDano = 0f;
        vidaAtual -= dano;

        if (vidaAtual <= 0f)
        {
            StartCoroutine(Atordoar());
        }
    }

    private IEnumerator Atordoar()
    {
        estaAtordoado = true;
        if (playerMov != null)
            playerMov.enabled = false;

        Debug.Log("🐶 Cachorro Atordoado!");

        yield return new WaitForSeconds(duracaoStun);

        vidaAtual = vidaMaxima;
        estaAtordoado = false;

        if (playerMov != null)
            playerMov.enabled = true;

        Debug.Log("🐶 Cachorro se recuperou!");
    }

    public bool EstaAtordoado() => estaAtordoado;
}
