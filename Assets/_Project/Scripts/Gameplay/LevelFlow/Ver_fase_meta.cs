using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Ver_fase_meta : MonoBehaviour
{
    string sceneName;
    private Text texto_meta;
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        texto_meta = this.gameObject.GetComponent<Text>();
    }
    private void Update()
    {

        if (sceneName == "Tutorial")
        {
            texto_meta.text = "Tutorial";
        }
        if (sceneName == "Fase_TatuMafioso_01")
        {
            texto_meta.text = "Meta: 2 min";
        }
        else if (sceneName == "Fase_Alien_02")
        {
            texto_meta.text = "Meta: 3 min";
        }
        else if (sceneName == "Fase_Dino_03")
        {
            texto_meta.text = "Meta: 2:30 min";
        }
    }
}
