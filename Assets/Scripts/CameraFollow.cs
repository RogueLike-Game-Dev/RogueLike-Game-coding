using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player; //Need to know if it's facing right or not
    [SerializeField] private BoxCollider2D boxBounds; //Getting bounds from a square gameObject
    private Vector3 smoothPos;
    public float smoothSpeed = 0.5f;
    private Camera mainCamera;

    private float xMin, xMax, yMin, yMax;
    private float camY, camX;
    private float camOrthsize;
    private float cameraRatio;
    private bool facingRight = true;

    private void Start()
    {
        if (player == null) //If not set from inspector
            player = GameObject.Find("Player");
        if (boxBounds == null)
            boxBounds = GameObject.Find("Background").GetComponent<BoxCollider2D>(); //By default get bounds from background

        xMin = boxBounds.bounds.min.x;
        xMax = boxBounds.bounds.max.x;
        yMin = boxBounds.bounds.min.y;
        yMax = boxBounds.bounds.max.y;
        mainCamera = GetComponent<Camera>();
        camOrthsize = mainCamera.orthographicSize;
        cameraRatio = (xMax + camOrthsize) / 2.0f;

        //this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        
    }

    private void FixedUpdate()
    {
        camY = Mathf.Clamp(player.transform.position.y, yMin + camOrthsize, yMax - camOrthsize);
        camX = Mathf.Clamp(player.transform.position.x, xMin + cameraRatio, xMax - cameraRatio);
        smoothPos = Vector3.Lerp(this.transform.position, new Vector3(camX, camY, this.transform.position.z), smoothSpeed);
        this.transform.position = smoothPos;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.localPosition = new Vector3(transform.localPosition.x * -1, transform.localPosition.y, transform.position.z);
    }
}