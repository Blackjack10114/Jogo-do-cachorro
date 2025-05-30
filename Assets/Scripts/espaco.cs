using UnityEngine;

public class espaco : MonoBehaviour
{
    Vector3 Player;
    public Vector3 offset;
    public GameObject céu;
    private GameObject Fundoespaço;
    private bool Fundoespaçoinsta;
    public GameObject FadeOut;

    private void Start()
    {
        Fundoespaço = Resources.Load<GameObject>("Fundo_Espacial_0");
        offset = new Vector3(0f, -40, 0);
    }
    void Update()
    {
        Player = this.transform.position;
        if (Player.y >= 770 && !Fundoespaçoinsta)
        {
            Fundoespaço = Instantiate(Fundoespaço, this.transform.position + offset, Quaternion.identity);
            Fundoespaçoinsta = true;
            Fundoespaço.transform.parent = céu.transform;
            var m = Instantiate(FadeOut, transform.position, Quaternion.identity);
        }
        if (Player.y < 770 && Fundoespaçoinsta)
        {
            Fundoespaçoinsta = false;
            Destroy(Fundoespaço);
        }
    }
}
