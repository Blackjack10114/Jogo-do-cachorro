using UnityEngine;
using UnityEngine.UI;

public class TempoFase : MonoBehaviour
{
    public float tempoAtual = 0f;
    public Text Tempo_Text;

    void Update()
    {
        tempoAtual += Time.deltaTime;

        if (Tempo_Text != null)
            AtualizarTextoTempo();
    }


    void AtualizarTextoTempo()
    {
        int minutos = Mathf.FloorToInt(tempoAtual / 60);
        int segundos = Mathf.FloorToInt(tempoAtual % 60);
        Tempo_Text.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
