using UnityEngine;

public class Jogador_Plataforma : MonoBehaviour
{
    private GameObject target = null;
    private Vector3 offset;

    void Start()
    {
        target = null;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        target = col.gameObject;
        offset = target.transform.position - transform.position;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        target = null;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            target.transform.position = transform.position + offset;
        }

    }
}

