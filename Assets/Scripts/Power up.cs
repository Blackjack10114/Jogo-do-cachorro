using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { Turbo, Gourmet, Bolha, DoubleJump }
    public PowerUpType type;
    public float duration;     // Duração do efeito (definido no Inspector)
    public float turboMultiplier = 2f; // Multiplicador de velocidade do Turbo
    public float turboStaminaReduction = 0.5f; // Redução do consumo de stamina no Turbo (50%)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMov player = other.GetComponent<PlayerMov>();
            Dano dano = other.GetComponent<Dano>();

            if (player != null)
            {
                switch (type)
                {
                    case PowerUp.PowerUpType.Turbo:
                        player.isTurboActive = true;
                        player.turboTimer = duration;

                        player.turboMultiplier = 1.4f;      
                        player.turboStaminaReduction = 0.3f;

                        // recupera stamina
                        player.stamina = Mathf.Min(100f, player.stamina + 40f);
                        break;


                    case PowerUpType.Gourmet:
                        // Ativa o gourmet (ex: reduz consumo de stamina ou outro efeito)
                        player.isGourmetActive = true;
                        player.gourmetTimer = duration;
                        player.stamina = 100f;
                        Debug.Log("Gourmet ativado!");
                        break;

                    case PowerUpType.Bolha:
                        if (dano != null)
                            dano.ActivateShield();
                        break;

                    case PowerUpType.DoubleJump:
                        player.StartCoroutine(player.AtivarPuloDuploTemporario(duration));
                        break;


                }
            }
        }
    }
}
