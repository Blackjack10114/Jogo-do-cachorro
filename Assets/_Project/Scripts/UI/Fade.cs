using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Fade : MonoBehaviour
{
    public float duration = 2f;
    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, timer / duration);

        if (sr != null)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }

        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
