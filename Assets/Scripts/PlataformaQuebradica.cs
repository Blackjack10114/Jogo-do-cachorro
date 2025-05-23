using UnityEngine;
using System.Collections;

public class PlataformaQuebradiça : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    public float delayAntesDeQuebrar = 0.5f;
    public float delayAntesDeSumir = 0.3f;

    [Header("Sons")]
    public AudioClip somAviso;   // Som de aviso (range, tremor)
    public AudioClip somQuebrar; // Som da quebra
    public AudioSource audioSource;

    private bool quebrando = false;

    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Pega o AudioSource no próprio objeto ou nos filhos
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
        // 🔊 Som de aviso (começando a quebrar)
        if (audioSource != null && somAviso != null)
            audioSource.PlayOneShot(somAviso);

        yield return new WaitForSeconds(delayAntesDeQuebrar);

        // 🔊 Som da quebra
        if (audioSource != null && somQuebrar != null)
            audioSource.PlayOneShot(somQuebrar);

        col.enabled = false;
        sr.color = new Color(1, 1, 1, 0.5f); // efeito visual de "quebrando"

        yield return new WaitForSeconds(delayAntesDeSumir);
        gameObject.SetActive(false);
    }
}
