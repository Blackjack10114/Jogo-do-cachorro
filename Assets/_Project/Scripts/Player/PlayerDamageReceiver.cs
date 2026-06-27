using UnityEngine;
using System.Collections;

public class PlayerDamageReceiver : MonoBehaviour
{
    // Configurações para controle do atordoamento
    public int collisionThreshold = 3;          // Número de colisões para atordoar
    public float collisionTimeWindow = 3f;        // Tempo em que essas colisões devem ocorrer
    public float stunDuration = 3f;               // Tempo de atordoamento

    private int collisionCount = 0;
    private float collisionTimer = 0f;
    private bool isStunned = false;

    // Referência para o script de movimento, para desabilitar o movimento enquanto atordoado
    private PlayerMov playerMov;

    void Start()
    {
        playerMov = GetComponent<PlayerMov>();
    }

    void Update()
    {
        // Se houver colisões recentes, atualiza o timer
        if (collisionCount > 0)
        {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= collisionTimeWindow)
            {
                // Se passou o tempo sem colisões suficientes, zera a contagem
                collisionCount = 0;
                collisionTimer = 0f;
            }
        }
    }

    // Método a ser chamado por obstáculos
    public void ObstacleCollision(float damageAmount)
    {
        if (isStunned)
            return;

        collisionCount++;
        collisionTimer = 0f; // Reinicia o timer a cada colisão

        // Se quiser integrar com um sistema de vida para stun (por exemplo, subtraindo pontos de vida):
        // Você pode chamar: GetComponent<StunControllerComVida>().TomarDano(damageAmount);

        // Se o número de colisões atingir o limite, atordoa o cachorro
        if (collisionCount >= collisionThreshold)
        {
            StartCoroutine(StunPlayer());
        }
    }

    private IEnumerator StunPlayer()
    {
        isStunned = true;
        if (playerMov != null)
            playerMov.enabled = false;  // Impede que o jogador se mova

        Debug.Log("Cachorro atordoado por múltiplos danos!");

        yield return new WaitForSeconds(stunDuration);

        if (playerMov != null)
            playerMov.enabled = true;   // Libera o movimento após o stun

        // Reseta as variáveis de colisão e atordoamento
        collisionCount = 0;
        collisionTimer = 0f;
        isStunned = false;

        Debug.Log("Cachorro se recuperou do atordoamento!");
    }
}
