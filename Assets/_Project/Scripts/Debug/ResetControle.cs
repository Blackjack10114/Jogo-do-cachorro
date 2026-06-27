using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ResetDebugController : MonoBehaviour
{
    private float comboWindow = 0.5f; // tempo máximo entre os botões
    private float[] resetTimes;
    private float[] skipTimes;

    public float delayAposUso = 2.0f;
    private static float proximoUsoPermitido = 0f;

    private InputControl[] resetButtons;
    private InputControl[] skipButtons;

    // ? REMOVIDO: A sequência de fases não é mais necessária aqui.
    // O GerenciadorDeJogo já sabe a ordem correta.
    // private string[] sequenciaDeFases = { ... };

    private void Start()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.LogWarning("Nenhum controle conectado!");
            enabled = false;
            return;
        }

        // combinação pra RESET (sem alteração aqui)
        resetButtons = new InputControl[]
        {
            gamepad.leftStickButton, gamepad.rightStickButton, gamepad.leftTrigger,
            gamepad.rightTrigger, gamepad.leftShoulder, gamepad.rightShoulder
        };
        resetTimes = new float[resetButtons.Length];

        // combinação pra SKIP (sem alteração aqui)
        skipButtons = new InputControl[]
        {
            gamepad.startButton, gamepad.selectButton, gamepad.leftTrigger,
            gamepad.rightTrigger, gamepad.leftShoulder, gamepad.rightShoulder
        };
        skipTimes = new float[skipButtons.Length];
    }

    private void Update()
    {
        if (Time.time < proximoUsoPermitido)
        {
            return;
        }

        if (Gamepad.current == null) return;

        // --- Lógica de Reset ---
        for (int i = 0; i < resetButtons.Length; i++)
        {
            if (resetButtons[i].IsPressed())
                resetTimes[i] = Time.time;
        }

        if (AllPressedWithin(resetTimes, comboWindow))
        {
            GerenciadorDeJogo.ResetarProgresso();
            Debug.Log("Progresso resetado pelo controle!");
            SceneManager.LoadScene("MenuPrincipal");

            // Limpa os timers de AMBOS os combos
            ClearTimes(resetTimes);
            ClearTimes(skipTimes); 

            proximoUsoPermitido = Time.time + delayAposUso;
        }

        // --- Lógica de Skip ---
        for (int i = 0; i < skipButtons.Length; i++)
        {
            if (skipButtons[i].IsPressed())
                skipTimes[i] = Time.time;
        }

        if (AllPressedWithin(skipTimes, comboWindow))
        {
            Debug.Log("Skip de fase!");
            GerenciadorDeJogo.Instance.IrParaProximaFase();

            // Limpa os timers de AMBOS os combos
            ClearTimes(skipTimes);
            ClearTimes(resetTimes); 

            proximoUsoPermitido = Time.time + delayAposUso;
        }
    }

    // O resto do script (AllPressedWithin e ClearTimes) continua igual.
    // O método SkipToNextScene() foi removido pois não é mais usado.

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
}