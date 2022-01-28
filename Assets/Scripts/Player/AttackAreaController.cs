using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    [SerializeField] private EntityStats playerStats;
    [SerializeField] private GameObject parent;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + " " + parent.name);
        if (collision.gameObject == parent)
            return;

        if(collision.gameObject.TryGetComponent(out EntityStats collisionStats))
        {
            collisionStats.Damage(playerStats.DMG);
            Debug.Log("Attacked " + collision.gameObject.name + " Current Hp: " + collisionStats.currentHP);
        }
       

    }
}
