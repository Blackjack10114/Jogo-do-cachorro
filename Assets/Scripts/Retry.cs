using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Retry : MonoBehaviour
{
    [SerializeField] private Button ButtonRetry;

    private void Awake()
    {
        ButtonRetry.onClick.AddListener(RecarregarCena);
    }
    public void RecarregarCena()
    {
        Time.timeScale = 1f; // volta ao tempo normal
        SceneManager.LoadScene("SampleSceneHugo");
    }
}
