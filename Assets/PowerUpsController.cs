using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpsController : MonoBehaviour
{

    [SerializeField] private GameObject pickupEffect; 
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        Debug.Log("entered Pickup()");
        Instantiate(pickupEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    
}
