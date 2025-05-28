using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TempoFase : MonoBehaviour
{
    public float tempoAtual = 0f;
    private float TempoMeta;
    public Text Tempo_Text;
    string sceneName;
    private bool amarelo;
    private Color CorAmarela, CorVermelha, CorPreta;

    private void Start()
    {
        CorAmarela = new Color(255 / 255f, 200 / 255f, 37 / 255f, 1f);
        CorVermelha = new Color(202 / 255f, 17 / 255f, 46 / 255f, 1f);
        CorPreta = new Color(0 / 255f, 0 / 255f, 0 / 255f, 1f);
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }
    void Update()
    {
        tempoAtual += Time.deltaTime;
        verificar_fase();
        //Debug.Log(TempoMeta);
        if (Tempo_Text != null)
        {
            AtualizarTextoTempo();
            if (tempoAtual > TempoMeta)
            {
                Tempo_Text.color = CorVermelha;
                amarelo = false;
            }
            if ((TempoMeta - 30f) <= tempoAtual)
            {
                amarelo = true;
            }
            if (amarelo == true && tempoAtual < TempoMeta)
            {
                Tempo_Text.color = CorAmarela;
            }
            if (tempoAtual < TempoMeta && amarelo == false)
            {
                Tempo_Text.color = CorPreta;
            }
        }
    }


    void AtualizarTextoTempo()
    {
        int minutos = Mathf.FloorToInt(tempoAtual / 60);
        int segundos = Mathf.FloorToInt(tempoAtual % 60);
        Tempo_Text.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
    void verificar_fase()
    {
        if (sceneName == "Fase_TatuMafioso_01")
        {
            TempoMeta = 120f;
        }
        else if (sceneName == "Fase_Alien_02")
        {
            TempoMeta = 180f;
        }
        else if (sceneName == "Fase_Dino_03")
        {
            TempoMeta = 150f;
        }
    }
}
