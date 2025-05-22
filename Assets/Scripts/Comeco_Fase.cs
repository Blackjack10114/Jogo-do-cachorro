using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEditor.PackageManager.UI;
using UnityEngine.UI;

public class Comeco_Fase : MonoBehaviour
{
    [HideInInspector] public bool apertou_botao;
    void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(Deixarvisivel());
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            apertou_botao = true;
        }
        if (apertou_botao)
        {
            Time.timeScale = 1f;
            Destroy(this.gameObject);
        }
    }
    private IEnumerator Deixarvisivel()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        this.GetComponent<Text>().enabled = true;
        StartCoroutine(Deixarnaovisivel());
    }
    private IEnumerator Deixarnaovisivel()
    {
        yield return new WaitForSecondsRealtime(0.8f);
        this.GetComponent<Text>().enabled = false;
        StartCoroutine(Deixarvisivel());
    }
}
