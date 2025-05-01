using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassaroSpawner : MonoBehaviour
{
    public GameObject prefabPassaro;
    public Transform jogador;
    public float distanciaAtivacao = 5f;
    public float tempoRespawn = 8f;

    private float timer = 0f;
    private bool jogadorProximo = false;
    private bool passaroAtivo = false;

    void Update()
    {
        float distancia = Vector3.Distance(jogador.position, transform.position);

        // Ativa o temporizador se o jogador estiver perto
        if (distancia < distanciaAtivacao)
        {
            jogadorProximo = true;
        }

        // Se o jogador est� perto e o p�ssaro n�o est� ativo, come�a o respawn
        if (jogadorProximo && !passaroAtivo)
        {
            timer += Time.deltaTime;
            if (timer >= tempoRespawn)
            {
                SpawnarPassaro();
                timer = 0f;
            }
        }
    }

    void SpawnarPassaro()
    {
        GameObject novo = Instantiate(prefabPassaro, transform.position, Quaternion.identity);
        Passaro p = novo.GetComponent<Passaro>();
        if (p != null) p.jogador = jogador;
        passaroAtivo = true;

        // Quando o p�ssaro se auto-destruir, o spawner poder� criar outro
        StartCoroutine(EsperarDestruir(novo));
    }

    private IEnumerator EsperarDestruir(GameObject passaro)
    {
        yield return new WaitUntil(() => passaro == null);
        passaroAtivo = false;
    }
}
