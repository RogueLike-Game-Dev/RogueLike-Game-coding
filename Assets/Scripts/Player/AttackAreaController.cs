using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    [SerializeField] private EntityStats playerStats;
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        var collisionStats = collision.gameObject.GetComponent<EntityStats>();
        if (collisionStats != null)
        {
            collisionStats.Damage(playerStats.DMG);
            Debug.Log("Attacked " + collision.gameObject.name + " Current Hp: " + collisionStats.currentHP);
        }
       

    }
}
