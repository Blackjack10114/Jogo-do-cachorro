using UnityEngine;
using System.Collections;

public class PlataformaQuebradiça : MonoBehaviour
{
    public float delayAntesDeQuebrar = 0.5f;
    public float delayAntesDeSumir = 0.3f;
    private bool quebrando = false;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!quebrando && collision.gameObject.CompareTag("Player"))
        {
            quebrando = true;
            StartCoroutine(Quebrar());
        }
    }

    IEnumerator Quebrar()
    {
        yield return new WaitForSeconds(delayAntesDeQuebrar);
        col.enabled = false;
        sr.color = new Color(1, 1, 1, 0.5f); // efeito visual de "quebrando"

        yield return new WaitForSeconds(delayAntesDeSumir);
        gameObject.SetActive(false);
    }
}
