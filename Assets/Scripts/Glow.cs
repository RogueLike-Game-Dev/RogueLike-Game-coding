using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSize;

    private float timer;

    public float growTime;
    // Start is called before the first frame update
    void Start()
    {
        
        timer = 0f;
        

        StartCoroutine(GlowMotion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GlowMotion()
    {
        Vector2 startScale = transform.localScale;
        Vector2 maxScale = new Vector2(maxSize, maxSize);

        while (true)
        {
            do
            {
                transform.localScale = Vector2.Lerp(startScale, maxScale, timer / growTime);
                timer += Time.deltaTime;
                yield return null;
           
            } while (timer < growTime);

            timer = 0f;
           
            do
            {
                transform.localScale = Vector2.Lerp(maxScale, startScale, timer / growTime);
                timer += Time.deltaTime;
                yield return null;
           
            } while (timer < growTime); 
            
            timer = 0f;
        }
        

    }
}
