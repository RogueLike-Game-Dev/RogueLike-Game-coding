using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelPaula : MonoBehaviour
{
    private GameObject player;
    private GameObject mainCamera;
    public GameObject background;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        player.transform.position = new Vector3(-38.5f, -6.8f, 0.0f);
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        mainCamera = player.transform.GetChild(0).gameObject;
        mainCamera.transform.localPosition = new Vector3(9.440001f, 4.679058f, -10.0f);
        background.transform.parent = player.transform;
        player.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        player.GetComponent<Rigidbody2D>().mass = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
