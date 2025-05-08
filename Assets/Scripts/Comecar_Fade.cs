using UnityEngine;

public class Comecar_Fade : MonoBehaviour
{
    public GameObject FadeOut;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == ("Buraco"))
        {
            var m = Instantiate(FadeOut, transform.position, Quaternion.identity);
        }
    }
}
