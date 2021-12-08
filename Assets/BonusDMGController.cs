using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusDMGController : MonoBehaviour
{
    [SerializeField] private GameObject pickupEffect; 
    [SerializeField] private GameObject effect; 
    [SerializeField] private float duration; 
    [SerializeField] private int bonusDMG; 
   
    
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
        
        
        
        EntityStats stats = player.GetComponent<EntityStats>();
        stats.DMG += bonusDMG;
        AttackAreaController attackAreaController = player.transform.Find("AttackArea").GetComponent<AttackAreaController>();
        attackAreaController.SetEffect(effect);
        attackAreaController.SetShowEffect(true);
        
        yield return new WaitForSeconds(duration);

        attackAreaController.SetShowEffect(false);
        attackAreaController.SetEffect(null);

        stats.DMG -= bonusDMG;
        Debug.Log("Power up ended");
        Destroy(gameObject);
    }
}
