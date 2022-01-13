using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformChildController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("!!!!!!!! Enter " + col.gameObject.name + " !!!!!");
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("!!!!!!!! Enter " + col.gameObject.name + " !!!!!");
            col.transform.parent = transform;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("!!!!!!!! Exit " + col.gameObject.name + " !!!!!");
        if (col.CompareTag("Player"))
        {
            Debug.Log("!!!!!!!! Exit " + col.name + " !!!!!");
            col.transform.parent = null;
        }
    }
}
