using UnityEngine;

public class MeteoritoSpawner : MonoBehaviour
{
    public GameObject meteoritoPrefab;
    public GameObject avisoPrefab;   // sombra ou outro indicador
    public Transform jogador;
    public float tempoEntreQuedas = 3f;
    public float distanciaAtivacao = 12f;
    public float tempoAviso = 0.6f;

    private float timer;

    void Update()
    {
        if (jogador == null || meteoritoPrefab == null) return;

        if (Vector2.Distance(jogador.position, transform.position) > distanciaAtivacao)
            return;

        timer += Time.deltaTime;
        if (timer < tempoEntreQuedas) return;

        // instanciar aviso
        if (avisoPrefab != null)
        {
            var aviso = Instantiate(avisoPrefab, transform.position, Quaternion.identity);
            Destroy(aviso, tempoAviso);
        }

        // depois do aviso, cai o meteorito
        Invoke(nameof(SpawnarMeteorito), tempoAviso);
        timer = 0f;
    }

    void SpawnarMeteorito()
    {
        var m = Instantiate(meteoritoPrefab, transform.position, Quaternion.identity);
        var meteoro = m.GetComponent<Meteorito>();
        if (meteoro != null)
            meteoro.AtivarQueda();
    }
}
