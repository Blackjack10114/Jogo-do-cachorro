using UnityEngine;

public class espaco : MonoBehaviour
{
    Vector3 Player;
    public Vector3 offset;
    public GameObject c�u;
    private GameObject Fundoespa�o;
    private bool Fundoespa�oinsta;
    public GameObject FadeOut;

    private void Start()
    {
        Fundoespa�o = Resources.Load<GameObject>("Fundo_Espacial_0");
        offset = new Vector3(0f, -40, 0);
    }
    void Update()
    {
        Player = this.transform.position;
        if (Player.y >= 770 && !Fundoespa�oinsta)
        {
            Fundoespa�o = Instantiate(Fundoespa�o, this.transform.position + offset, Quaternion.identity);
            Fundoespa�oinsta = true;
            Fundoespa�o.transform.parent = c�u.transform;
            var m = Instantiate(FadeOut, transform.position, Quaternion.identity);
        }
        if (Player.y < 770 && Fundoespa�oinsta)
        {
            Fundoespa�oinsta = false;
            Destroy(Fundoespa�o);
        }
    }
}
