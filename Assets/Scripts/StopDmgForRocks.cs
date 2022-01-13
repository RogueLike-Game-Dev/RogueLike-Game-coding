using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDmgForRocks : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Rock"))
        {
            //spawned rocks should damage the player only if it hits his head
            if (this.gameObject.TryGetComponent<EntityStats>(out EntityStats script))
            {
                script.enabled = false;
                Destroy(script);
            }
            
        }
    }
}
