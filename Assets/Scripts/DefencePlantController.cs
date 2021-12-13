using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefencePlantController : MonoBehaviour
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
        
        GameObject effect = Instantiate(playerEffect, player.transform.position , player.transform.rotation);
        effect.transform.SetParent(player.transform);
        EntityStats stats = player.GetComponent<EntityStats>();

        int hp = stats.HPPowerUp();
        
        yield return new WaitForSeconds(duration);

        stats.HPResetPowerUp(hp);
        Destroy(effect);
        Debug.Log("Power up ended");
        Destroy(gameObject);
    }
}
