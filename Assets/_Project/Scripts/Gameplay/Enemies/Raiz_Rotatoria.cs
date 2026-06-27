using UnityEngine;

public class Raiz_Rotatoria : MonoBehaviour
{
    public enum TipoRotacao { Oscilacao, RotacaoCompleta }
    public TipoRotacao tipo = TipoRotacao.Oscilacao;

    [Header("Configurações Gerais")]
    public float velocidade = 2f;

    [Header("Oscilação")]
    public float amplitude = 45f;
    private float anguloInicial;

    void Start()
    {
        anguloInicial = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        switch (tipo)
        {
            case TipoRotacao.Oscilacao:
                Oscilar();
                break;
            case TipoRotacao.RotacaoCompleta:
                RotacionarCompleto();
                break;
        }
    }

    void Oscilar()
    {
        float angulo = amplitude * Mathf.Sin(Time.time * velocidade);
        transform.rotation = Quaternion.Euler(0, 0, anguloInicial + angulo);
    }

    void RotacionarCompleto()
    {
        transform.Rotate(0, 0, velocidade * Time.deltaTime * 360f); // 360f para uma volta por segundo com velocidade = 1
    }
}
