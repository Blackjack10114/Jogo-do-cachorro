using System.Collections;
using UnityEngine;

public class Playercheckpoint : MonoBehaviour
{
    private Vector2 lastCheckpointPosition;
    private Caixa bool_script;

    void Start()
    {
        lastCheckpointPosition = transform.position;
        bool_script = this.gameObject.GetComponent<Caixa>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            lastCheckpointPosition = other.transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Buraco"))
        {
            transform.position = lastCheckpointPosition;
            StartCoroutine(HabilitarCaixa());
        }
    }
    private IEnumerator HabilitarCaixa()
    {
        yield return new WaitForSeconds(0.1f);

        if (bool_script != null)
        {
            bool_script.ForcarRetornoCaixaDoBuraco();
        }
    }

}
