using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//TODO: see Diana's and Paula's
public class LevelManagerTavi : MonoBehaviour
{

    private EntityStats playerStats;
    private GameObject player;
    private GameObject mainCamera;

    void Start()
    {
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<EntityStats>();
        // player.transform.position = new Vector3(0f, -1.44f, 0.0f);
        player.GetComponent<PlayerMovement>().jumpForce = 16;

        // mainCamera = player.transform.GetChild(0).gameObject;
        // mainCamera.transform.localPosition = new Vector3(3.05f, 1.0f, -10.0f);
        // mainCamera.GetComponent<Camera>().orthographicSize = 5;

        // var minimap = mainCamera.transform.GetChild(0).gameObject;
        // minimap.transform.localPosition = new Vector3(5.0f, 3.0f, 0.0f);
        // minimap.GetComponent<Camera>().orthographicSize = 9;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
