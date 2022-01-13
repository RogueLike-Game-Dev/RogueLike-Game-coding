using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerTavi : MonoBehaviour
{

    public GameObject nextLevelDoor;

    private EntityStats playerStats;
    private GameObject player;
    private GameObject mainCamera;

    void Start()
    {
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<EntityStats>();
        player.transform.position = new Vector3(0f, -1.44f, 0.0f);
        player.GetComponent<PlayerMovement>().jumpForce = 16;

        mainCamera = player.transform.GetChild(0).gameObject;
        mainCamera.transform.localPosition = new Vector3(3.05f, 3.0f, -10f);
        mainCamera.GetComponent<Camera>().orthographicSize = 10;

        var minimap = mainCamera.transform.GetChild(0).gameObject;
        minimap.transform.localPosition = new Vector3(5.0f, 3.0f, 0.0f);
        minimap.GetComponent<Camera>().orthographicSize = 13;

        nextLevelDoor.GetComponent<LoadNextRoom>().enabled = true; //is inaccessible until player finishes level anyway
    }

}
