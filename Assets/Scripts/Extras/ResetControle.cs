using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ResetDebugController : MonoBehaviour
{
    private float comboWindow = 0.5f; // tempo máximo entre os botões
    private float[] resetTimes;
    private float[] skipTimes;

    // botões exigidos pro reset
    private InputControl[] resetButtons;
    // botões exigidos pro skip
    private InputControl[] skipButtons;

    private string[] sequenciaDeFases = {
        "Tutorial",
        "Fase_TatuMafioso_01",
        "Fase_Alien_02",
        "Fase_Dino_03"};
    private void Start()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.LogWarning("Nenhum controle conectado!");
            enabled = false;
            return;
        }

       

    // combinação pra RESET (sticks + triggers + shoulders)
    resetButtons = new InputControl[]
        {
            gamepad.leftStickButton,
            gamepad.rightStickButton,
            gamepad.leftTrigger,
            gamepad.rightTrigger,
            gamepad.leftShoulder,
            gamepad.rightShoulder
        };
        resetTimes = new float[resetButtons.Length];

        // combinação pra SKIP (start + select + triggers + shoulders)
        skipButtons = new InputControl[]
        {
            gamepad.startButton,
            gamepad.selectButton,
            gamepad.leftTrigger,
            gamepad.rightTrigger,
            gamepad.leftShoulder,
            gamepad.rightShoulder
        };
        skipTimes = new float[skipButtons.Length];
    }

  
    private void Update()
    {
        if (Gamepad.current == null) return;

        // escuta reset
        for (int i = 0; i < resetButtons.Length; i++)
        {
            if (resetButtons[i].IsPressed())
                resetTimes[i] = Time.time;
        }
        if (AllPressedWithin(resetTimes, comboWindow))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Progresso resetado pelo controle!");
            SceneManager.LoadScene("MenuPrincipal");
            ClearTimes(resetTimes);
        }

        // escuta skip
        for (int i = 0; i < skipButtons.Length; i++)
        {
            if (skipButtons[i].IsPressed())
                skipTimes[i] = Time.time;
        }
        if (AllPressedWithin(skipTimes, comboWindow))
        {
            Debug.Log("Skip de fase!");
            SkipToNextScene();
            ClearTimes(skipTimes);
        }
    }

    private bool AllPressedWithin(float[] times, float window)
    {
        float latest = 0f;
        float earliest = float.MaxValue;

        for (int i = 0; i < times.Length; i++)
        {
            if (times[i] == 0) return false;
            if (times[i] > latest) latest = times[i];
            if (times[i] < earliest) earliest = times[i];
        }

        return (latest - earliest) <= window;
    }

    private void ClearTimes(float[] times)
    {
        for (int i = 0; i < times.Length; i++)
            times[i] = 0;
    }

    private void SkipToNextScene()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        int indiceAtual = -1;

        for (int i = 0; i < sequenciaDeFases.Length; i++)
        {
            if (sequenciaDeFases[i] == cenaAtual)
            {
                indiceAtual = i;
                break;
            }
        }

        if (indiceAtual == -1)
        {
            SceneManager.LoadScene(sequenciaDeFases[0]);
            return;
        }

        // Verifica se a cena atual é a última da lista
        if (indiceAtual == sequenciaDeFases.Length - 1)
        {
            Debug.Log("Última fase da sequência. O skip não fará nada.");
            return; // Simplesmente sai da função
        }
        // --- FIM DA LÓGICA NOVA ---

        // Se não for a última, continua com o skip normal
        int proximoIndice = indiceAtual + 1; // Não precisamos mais do "%" aqui
        string proximaCena = sequenciaDeFases[proximoIndice];

        Debug.Log($"Skipando de '{cenaAtual}' para '{proximaCena}'");
        SceneManager.LoadScene(proximaCena);
    }
}
