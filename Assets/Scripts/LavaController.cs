using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
            this.GetComponent<SpriteRenderer>().flipX = !this.GetComponent<SpriteRenderer>().flipX;
            timer += period;
        }
        Debug.Log(this.tag);
    }
}
