using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassaroSpawner : MonoBehaviour
{
    public GameObject prefabPassaro;
    public Transform jogador;
    public float distanciaAtivacao = 1000f;
    public float tempoRespawn = 2f;

    private float timer = 0f;

    public int maxPassaros = 3;
    private List<GameObject> passarosAtivos = new List<GameObject>();

    void Update()
    {
        float distancia = Vector3.Distance(jogador.position, transform.position);

        // Remove da lista pássaros que já foram destruídos
        passarosAtivos.RemoveAll(p => p == null);

        if (distancia < distanciaAtivacao && passarosAtivos.Count < maxPassaros)
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
        passarosAtivos.Add(novo);
    }
}

