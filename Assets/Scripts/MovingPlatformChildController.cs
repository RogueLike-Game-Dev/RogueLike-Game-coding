using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformChildController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.transform.parent = null;
        }
    }
}
