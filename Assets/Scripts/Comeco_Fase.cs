using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEditor.PackageManager.UI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Comeco_Fase : MonoBehaviour
{
    [HideInInspector] public bool apertou_botao, Semmeta;
    private GameObject fundoPrefab, metaPrefab;
    public Image barra_vida;
    void Start()
    {
        Time.timeScale = 0f;
        barra_vida.enabled = false;
        fundoPrefab = Resources.Load<GameObject>("Fundo");
        metaPrefab = Resources.Load<GameObject>("Texto_meta");
        metaPrefab = Instantiate(metaPrefab, this.transform.position, Quaternion.identity);
        fundoPrefab = Instantiate(fundoPrefab, this.transform.position, Quaternion.identity);
        metaPrefab.transform.parent = this.transform;
        metaPrefab.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(Tirarmeta());
    }
    void Update()
    {
        if (Input.anyKeyDown && Semmeta)
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
        yield return new WaitForSecondsRealtime(1f);
        this.GetComponent<Text>().enabled = true;
        StartCoroutine(Deixarnaovisivel());
    }
    private IEnumerator Deixarnaovisivel()
    {
        yield return new WaitForSecondsRealtime(1f);
        this.GetComponent<Text>().enabled = false;
        StartCoroutine(Deixarvisivel());
    }
    private IEnumerator Tirarmeta()
    {
        yield return new WaitForSecondsRealtime(4f);
        Destroy(metaPrefab);
        Destroy(fundoPrefab);
        barra_vida.enabled = true;
        Semmeta = true;
        StartCoroutine(Deixarvisivel());
    }
}
