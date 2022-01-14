using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : MonoBehaviour
{

    private GameObject player;
    private GameObject mainCamera;
    public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        player.transform.position = new Vector3(-9.1f, -7);
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        player.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        player.GetComponent<Rigidbody2D>().mass = 2.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
