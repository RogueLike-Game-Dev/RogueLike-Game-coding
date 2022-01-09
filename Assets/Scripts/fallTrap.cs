using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallTrap : MonoBehaviour
{
    public int damage;

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
        yield return new WaitForSeconds(1.15f);
        
        player.GetComponent<EntityStats>().Damage(damage);
        var playerTransform = player.transform;
        playerTransform.position = new Vector3(122.66f, 2.5f, 0f);

        //unfreeze player position and make visible


    }

}
