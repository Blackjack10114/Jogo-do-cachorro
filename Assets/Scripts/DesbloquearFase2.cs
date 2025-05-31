using UnityEngine;

public class DesbloquearFase2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GerenciadorDeJogo.Instance.DesbloquearFase(2);
    }
}