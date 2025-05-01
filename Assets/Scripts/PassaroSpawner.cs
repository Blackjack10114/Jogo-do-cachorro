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

        // Se o jogador está perto e o pássaro não está ativo, começa o respawn
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

        // Quando o pássaro se auto-destruir, o spawner poderá criar outro
        StartCoroutine(EsperarDestruir(novo));
    }

    private IEnumerator EsperarDestruir(GameObject passaro)
    {
        yield return new WaitUntil(() => passaro == null);
        passaroAtivo = false;
    }
}
