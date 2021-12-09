using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusHealController : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect; 
    [SerializeField] private GameObject playerEffect; 
    [SerializeField] private float duration; 
   
    
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            StartCoroutine( Pickup(other));
        }
    }

    private IEnumerator Pickup(Collider2D player)
    {
        Debug.Log("entered Pickup()");
        Instantiate(pickupEffect, transform.position, transform.rotation);
        
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(transform.Find("Fireball 2").gameObject);
        
        GameObject effect = Instantiate(playerEffect, player.transform.position + new Vector3(0,1f,0), player.transform.rotation);
        effect.transform.SetParent(player.transform);
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        playerMovement.SetDoubleHeal(true);
        
        yield return new WaitForSeconds(duration);
        
        playerMovement.SetDoubleHeal(false);
        Destroy(effect);
        Debug.Log("Power up ended");
        Destroy(gameObject);
    }
}
