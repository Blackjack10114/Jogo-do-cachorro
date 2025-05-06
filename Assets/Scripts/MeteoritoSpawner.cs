using UnityEngine;

public class MeteoritoSpawner : MonoBehaviour
{
    public GameObject meteoritoPrefab;
    public GameObject avisoPrefab;
    public Transform jogador;

    public float tempoEntreQuedas = 3f;
    public float distanciaAtivacao = 12f;
    public float tempoAviso = 0.6f;

    private float timer = 0f;

    void Update()
    {
        if (jogador == null || meteoritoPrefab == null)
            return;

        float distancia = Vector2.Distance(jogador.position, transform.position);
        if (distancia > distanciaAtivacao)
            return;

        timer += Time.deltaTime;

        if (timer >= tempoEntreQuedas)
        {
            Vector3 posicao = transform.position;

            if (avisoPrefab != null)
            {
                GameObject aviso = Instantiate(avisoPrefab, posicao, Quaternion.identity);
                Destroy(aviso, tempoAviso);
            }

            Invoke(nameof(SpawnarMeteorito), tempoAviso);
            timer = 0f;
        }
    }

    void SpawnarMeteorito()
    {
        GameObject m = Instantiate(meteoritoPrefab, transform.position, Quaternion.identity);
        Meteorito meteoro = m.GetComponent<Meteorito>();
        if (meteoro != null)
            meteoro.AtivarQueda();
    }
}
