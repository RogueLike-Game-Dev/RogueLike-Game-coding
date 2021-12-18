using UnityEngine;

public class LavaController : MonoBehaviour
{
    private float timer;
    private float period;
    // Start is called before the first frame update
    void Start() 
    {
        timer = 0.0f;
        period = 0.5f;
    }

    // Update is called once per frame
    void Update() 
    {
        if (Time.time > timer) 
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            timer += period;
        }
    }
}
