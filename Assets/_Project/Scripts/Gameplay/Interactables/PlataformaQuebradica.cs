using UnityEngine;
using System.Collections;

public class PlataformaQuebradiça : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    public float delayAntesDeQuebrar = 0.5f;
    public float delayAntesDeSumir = 0.3f;

    [Header("Reaparecimento")]
    public float tempoParaReaparecer = 4f;
    public float duracaoFadeReaparecer = 0.5f;

    [Header("Sons")]
    public AudioClip somAviso;
    public AudioClip somQuebrar;
    public AudioSource audioSource;

    private bool quebrando = false;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = GetComponentInChildren<AudioSource>();
        }
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
        if (audioSource && somAviso)
            audioSource.PlayOneShot(somAviso);

        yield return new WaitForSeconds(delayAntesDeQuebrar);

        if (audioSource && somQuebrar)
            audioSource.PlayOneShot(somQuebrar);

        col.enabled = false;
        sr.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(delayAntesDeSumir);

        sr.enabled = false;

        yield return new WaitForSeconds(tempoParaReaparecer);

        yield return StartCoroutine(ReaparecerComFade());
    }
    IEnumerator ReaparecerComFade()
    {
        sr.enabled = true;
        col.enabled = false;

        float tempo = 0f;
        Color cor = sr.color;
        cor.a = 0f;
        sr.color = cor;

        while (tempo < duracaoFadeReaparecer)
        {
            tempo += Time.deltaTime;
            cor.a = Mathf.Lerp(0f, 1f, tempo / duracaoFadeReaparecer);
            sr.color = cor;
            yield return null;
        }

        cor.a = 1f;
        sr.color = cor;

        col.enabled = true;
        quebrando = false;
    }

   /* void Reaparecer()
    {
        sr.enabled = true;
        col.enabled = true;
        sr.color = Color.white;
        quebrando = false;
    }
   */
}
