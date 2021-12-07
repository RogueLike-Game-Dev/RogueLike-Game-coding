using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player; // Need to know if it's facing right or not
    [SerializeField] private BoxCollider2D boxBounds; // Getting bounds from a square gameObject
    private Vector3 smoothPos;
    public float smoothSpeed = 0.5f;
    private Camera mainCamera;

    private float xMin, xMax, yMin, yMax;
    private float camY, camX;
    private float camOrthsize;
    private float cameraRatio;
    private Vector3 playerPosition;
    private Vector3 cameraPosition;
    
    private void Start()
    {
        if (player == null)     // If not set from inspector
            player = GameObject.Find("Player");
        if (boxBounds == null)
            boxBounds = GameObject.Find("Background").GetComponent<BoxCollider2D>(); // By default get bounds from background

        var box = boxBounds.bounds;
        xMin = box.min.x;
        xMax = box.max.x;
        yMin = box.min.y;
        yMax = box.max.y;
        mainCamera = GetComponent<Camera>();
        camOrthsize = mainCamera.orthographicSize;
        cameraRatio = (xMax + camOrthsize) / 2.0f;
        playerPosition = player.transform.position;
        cameraPosition = transform.position;
    }

    private void FixedUpdate()
    {

        camY = Mathf.Clamp(playerPosition.y, yMin + camOrthsize, yMax - camOrthsize);
        camX = Mathf.Clamp(playerPosition.x, xMin + cameraRatio, xMax - cameraRatio);
        smoothPos = Vector3.Lerp(cameraPosition, new Vector3(camX, camY, cameraPosition.z), smoothSpeed);
        cameraPosition = smoothPos;

       // camY = Mathf.Clamp(player.transform.position.y, yMin + camOrthsize, yMax - camOrthsize);
        //camX = Mathf.Clamp(player.transform.position.x, xMin + camOrthsize*mainCamera.aspect, xMax - camOrthsize * mainCamera.aspect);
        //smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, this.transform.position.z), smoothSpeed);
        //this.transform.position = smoothPos;
    }

}
