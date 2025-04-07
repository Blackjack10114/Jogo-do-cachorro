using UnityEngine;

public class Caixa : MonoBehaviour
{
    private GameObject Player = null;
    private GameObject caixaPrefab = null; // Referência ao prefab da caixa
    private GameObject Caixa_Separada_0 = null; // Instância da caixa
    public float speed, move;
    private Rigidbody2D rb; // Rigidbody do jogador
    [SerializeField] public float jumpforce;
    private float time = 0f;
    public Sprite Sprite_Dog_Caixa_Normal;
    private Dano bool_script;
    public GameObject Sprite_Dog_Caixa_Normal_0;

    // Tornar a flag estática para garantir que a caixa seja instanciada uma única vez
    public bool caixaInstanciada = false;

    void Start()
    {
        // Garantir que o player tenha sido encontrado
        Player = GameObject.FindGameObjectWithTag("Player");

        // Carregar o prefab da caixa
        caixaPrefab = Resources.Load<GameObject>("Caixa_Separada_0");

        // Obter o Rigidbody2D do jogador
        rb = Player.GetComponent<Rigidbody2D>();
        
        bool_script = Sprite_Dog_Caixa_Normal_0.GetComponent<Dano>();
    }

    void Update()
    {
        // Verificar se o jogador e a caixa foram encontrados
        if (Player != null && Caixa_Separada_0 != null)
        {
            time += Time.deltaTime;
            if (time < 0.5f)
            {
                Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Caixa_Separada_0.GetComponent<Collider2D>());
            }
            else
            {
                Physics2D.IgnoreCollision(Player.GetComponent<Collider2D>(), Caixa_Separada_0.GetComponent<Collider2D>(), false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar se a colisão foi com a tag "Spike"
        if (collision.gameObject.CompareTag("Spike"))
        {
            {
                if (caixaPrefab != null && !caixaInstanciada)
                {
                    if (bool_script != null && bool_script.isInvincible == false)
                    {
                        Caixa_Separada_0 = Instantiate(caixaPrefab, Player.transform.position, Quaternion.identity);

                        // Obter o Rigidbody2D da caixa instanciada
                        Rigidbody2D caixaRb = Caixa_Separada_0.GetComponent<Rigidbody2D>();

                        if (caixaRb != null)
                        {
                            // Marcar a caixa como instanciada
                            caixaInstanciada = true;

                            // Aplicar a velocidade na caixa
                            caixaRb.linearVelocity = new Vector2(-move * speed, caixaRb.linearVelocity.y); // Velocidade horizontal

                            // Aplicar o impulso para cima na caixa
                            caixaRb.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }
        if (collision.gameObject.CompareTag("caixa"))
        {
            Destroy(Caixa_Separada_0);
            time = 0;
            caixaInstanciada = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite_Dog_Caixa_Normal;
        }
    }
}
