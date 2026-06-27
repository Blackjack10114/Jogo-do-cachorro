using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetDebug : MonoBehaviour
{
    void Update()
    {
        // Ir para fases
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKeyDown(KeyCode.F1))
                SceneManager.LoadScene("Tutorial");
            else if (Input.GetKeyDown(KeyCode.F2))
                SceneManager.LoadScene("Fase_TatuMafioso_01");
            else if (Input.GetKeyDown(KeyCode.F3))
                SceneManager.LoadScene("Fase_Alien_02");
            else if (Input.GetKeyDown(KeyCode.F4))
                SceneManager.LoadScene("Fase_Dino_03");
        }

        // Resetar progresso (Shift + F12)
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) &&
            Input.GetKeyDown(KeyCode.F12))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            Debug.Log("Progresso resetado.");
            SceneManager.LoadScene("MenuPrincipal");
        }
    }
}
