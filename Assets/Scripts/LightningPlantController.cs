using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningPlantController : MonoBehaviour
{

    [SerializeField] private GameObject pickupEffect; 
    [SerializeField] private GameObject lightningEffect; 
    [SerializeField] private float duration; 
    [SerializeField] private float distance; 
    [SerializeField] private int damage; 
    [SerializeField] private float lightningCooldown; 
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
        

        float timer = 0f;
        float lightningCooldownTimer = lightningCooldown + 1f;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        while (timer < duration)
        {
            
            if(lightningCooldownTimer > lightningCooldown)
            {
                
                foreach (GameObject enemy in enemies)
                {
                    if (isNear(player.gameObject, enemy))
                    {
                        Debug.Log(enemy.name + "is struck by lightning");
                        Instantiate(lightningEffect, enemy.transform.position, enemy.transform.rotation).transform.SetParent(enemy.transform);
                        enemy.GetComponent<EntityStats>().Damage(damage);
                        lightningCooldownTimer = 0f;
                    }
                }

            }
            
            timer += Time.deltaTime;
            lightningCooldownTimer += Time.deltaTime;
            yield return null;
        }
        
        /*GameObject[] lightningLeftovers = GameObject.FindGameObjectsWithTag("Lightning");
        foreach (GameObject lightning in lightningLeftovers)
        {
            Destroy(lightning);
        }*/
        Debug.Log("Power up ended");
        Destroy(gameObject);
    }

    private bool isNear(GameObject player, GameObject ob)
    {
        return ((player.transform.position.x + distance > ob.transform.position.x) &&
                (player.transform.position.x - distance < ob.transform.position.x) &&
                (player.transform.position.y + distance > ob.transform.position.y) &&
                (player.transform.position.y - distance < ob.transform.position.y));
    }

    
}
