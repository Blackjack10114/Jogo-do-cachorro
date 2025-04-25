using UnityEngine;

public class Raiz_Rotatoria : MonoBehaviour
{
    public float amplitude = 45f;
    public float velocidade = 2f;
    private float anguloinicial;
    
    void Start()
    {
        anguloinicial = transform.rotation.eulerAngles.z;
  
    }


    
    void Update()
    {
        float angulo = amplitude * Mathf.Sin(Time.time * velocidade);
        transform.rotation = Quaternion.Euler(0, 0, anguloinicial + angulo);
    }
}

