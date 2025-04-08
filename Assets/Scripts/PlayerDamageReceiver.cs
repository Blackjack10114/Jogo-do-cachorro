using UnityEngine;
using System.Collections;

public class PlayerDamageReceiver : MonoBehaviour
{
    // Configura��es para controle do atordoamento
    public int collisionThreshold = 3;          // N�mero de colis�es para atordoar
    public float collisionTimeWindow = 3f;        // Tempo em que essas colis�es devem ocorrer
    public float stunDuration = 3f;               // Tempo de atordoamento

    private int collisionCount = 0;
    private float collisionTimer = 0f;
    private bool isStunned = false;

    // Refer�ncia para o script de movimento, para desabilitar o movimento enquanto atordoado
    private PlayerMov playerMov;

    void Start()
    {
        playerMov = GetComponent<PlayerMov>();
    }

    void Update()
    {
        // Se houver colis�es recentes, atualiza o timer
        if (collisionCount > 0)
        {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= collisionTimeWindow)
            {
                // Se passou o tempo sem colis�es suficientes, zera a contagem
                collisionCount = 0;
                collisionTimer = 0f;
            }
        }
    }

    // M�todo a ser chamado por obst�culos
    public void ObstacleCollision(float damageAmount)
    {
        if (isStunned)
            return;

        collisionCount++;
        collisionTimer = 0f; // Reinicia o timer a cada colis�o

        // Se quiser integrar com um sistema de vida para stun (por exemplo, subtraindo pontos de vida):
        // Voc� pode chamar: GetComponent<StunControllerComVida>().TomarDano(damageAmount);

        // Se o n�mero de colis�es atingir o limite, atordoa o cachorro
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

        Debug.Log("Cachorro atordoado por m�ltiplos danos!");

        yield return new WaitForSeconds(stunDuration);

        if (playerMov != null)
            playerMov.enabled = true;   // Libera o movimento ap�s o stun

        // Reseta as vari�veis de colis�o e atordoamento
        collisionCount = 0;
        collisionTimer = 0f;
        isStunned = false;

        Debug.Log("Cachorro se recuperou do atordoamento!");
    }
}
