using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallTrap : MonoBehaviour
{
    public int damage;
    public Vector3 resetPosition; //The position the player object should be rest to

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject;
            StartCoroutine(ResetPlayerPosition(player));
        }
    }

    private IEnumerator ResetPlayerPosition(GameObject player)
    {
        //freeze player position and make invisible
        var prevConstraitns = player.GetComponent<Rigidbody2D>().constraints;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1.15f);
        
        player.GetComponent<EntityStats>().Damage(damage);
        var playerTransform = player.transform;
        playerTransform.position = resetPosition; 

        //unfreeze player position and make visible
        player.GetComponent<Rigidbody2D>().constraints = prevConstraitns;
        player.GetComponent<SpriteRenderer>().enabled = true;

    }

}
