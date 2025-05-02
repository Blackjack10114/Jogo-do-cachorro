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
                    case PowerUpType.Turbo:
                        // Ativa o turbo e define os modificadores
                        player.isTurboActive = true;
                        player.turboMultiplier = this.turboMultiplier;
                        player.turboStaminaReduction = this.turboStaminaReduction; // Aplica a redução no consumo de stamina
                        player.turboTimer = duration;
                        break;

                    case PowerUpType.Gourmet:
                        // Ativa o gourmet (ex: reduz consumo de stamina ou outro efeito)
                        player.isGourmetActive = true;
                        player.gourmetTimer = duration;
                        Debug.Log("Gourmet ativado!");
                        break;

                    case PowerUpType.Bolha:
                        if (dano != null)
                            StartCoroutine(dano.ActivateShield(duration));
                        break;

                    case PowerUpType.DoubleJump:
                        player.StartCoroutine(player.AtivarPuloDuploTemporario(duration));
                        break;


                }
            }
        }
    }
}
