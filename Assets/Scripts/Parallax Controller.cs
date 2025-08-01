using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public Transform Cam;
    Vector3 CamStartPosition;
    float Distance;

    GameObject[] backgrounds;
    Material[] mat;
    float[] backspeed;

    float farthestBack;

    [Range(0.01f, 0.5f)]
    public float parallaxspeed;

    void Start()
    {

        
        CamStartPosition = Cam.position;

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backspeed = new float[backCount];

        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {

            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;

        }

        BackSpeedCalculate(backCount);

    }

    void BackSpeedCalculate(int backCount)
    {

        for (int i = 0; i < backCount; i++)
        {

            if ((backgrounds[i].transform.position.z - Cam.position.z) > farthestBack)
            {

                farthestBack = backgrounds[i].transform.position.z - Cam.position.z;
            }

        }


        for (int i = 0; i < backCount; i++)
        {

            backspeed[i] = 1 - (backgrounds[i].transform.position.z - Cam.position.z) / farthestBack;

        }

    }

    private void LateUpdate()
    {
        Distance = Cam.position.x - CamStartPosition.x;
        transform.position = new Vector3(Cam.position.x, transform.position.y, 0);

        for (int i = 0; i < backgrounds.Length; i++)
        {

            float speed = backspeed[i] * parallaxspeed;
            mat[i].SetTextureOffset("_MainTex", new Vector2(Distance, 0) * speed);

        }
    }
}
