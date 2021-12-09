using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MysteryPowerUpController : MonoBehaviour
{
    [SerializeField] private GameObject powerUp1; 
    [SerializeField] private GameObject powerUp2; 
    [SerializeField] private GameObject powerUp3; 
    [SerializeField] private GameObject powerUp4; 
    [SerializeField] private GameObject powerUp5; 
    [SerializeField] private GameObject powerUp6; 
    
    
    
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
             Pickup(other);
        }
    }

    private void Pickup(Collider2D player)
    {
        Debug.Log("entered Pickup()");
        
        
        GetComponent<CircleCollider2D>().enabled = false;
        
        Destroy(transform.Find("Fireball 2").gameObject);
        Destroy(transform.Find("Question").gameObject);
        List<GameObject> powerUps = new List<GameObject>();

        powerUps.Add(powerUp1);
        powerUps.Add(powerUp2);
        powerUps.Add(powerUp3);
        powerUps.Add(powerUp4);
        powerUps.Add(powerUp5);
        powerUps.Add(powerUp6);

        

        System.Random rand = new System.Random();
        int index = rand.Next(powerUps.Count);
        Debug.Log(index);
        Instantiate(powerUps[index], transform.position, transform.rotation);
        
        
        
        Debug.Log("Power up ended");
        Destroy(gameObject);
        
    }
}
