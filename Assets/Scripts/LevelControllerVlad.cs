using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelControllerVlad : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject player;
    private EntityStats stats;
    private PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        stats = player.GetComponent<EntityStats>();
        playerMovement = player.GetComponent<PlayerMovement>();
        player.transform.position = new Vector3(-6f, -3.2f, 0.0f);
        //player.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        mainCamera = player.transform.GetChild(0).gameObject;
        mainCamera.transform.localPosition = new Vector3(9.440001f, 4.679058f, -10.0f);
        player.GetComponent<Rigidbody2D>().gravityScale = 1.5f;
        player.GetComponent<Rigidbody2D>().mass = 2f;
        playerMovement.jumpForce = 17;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
