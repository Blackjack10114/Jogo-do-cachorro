using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public AnimatorOverrideController overrideComCaixa;
    public AnimatorOverrideController overrideSemCaixa;

    private Animator anim;
    private Caixa caixaScript;

    void Start()
    {
        anim = GetComponent<Animator>();
        caixaScript = GetComponent<Caixa>();
    }

    void Update()
    {
        if (caixaScript != null && anim != null)
        {
            if (caixaScript.CaixaPega)
            {
                anim.runtimeAnimatorController = overrideComCaixa;
            }
            else
            {
                anim.runtimeAnimatorController = overrideSemCaixa;
            }
        }

        // Atualiza os parâmetros
        anim.SetBool("EstaAndando", Mathf.Abs(GetComponent<Rigidbody2D>().linearVelocity.x) > 0.1f);
        anim.SetBool("EstaPulando", !GetComponent<Jump>().EstaNoChao);
    }
}