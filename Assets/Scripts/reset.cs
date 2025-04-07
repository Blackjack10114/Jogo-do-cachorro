using UnityEngine;
using UnityEngine.SceneManagement;

public class reset : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene("SampleScene_Gab");
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            transform.position = new Vector2(-6f, 0.2f);
        }
    }
}
