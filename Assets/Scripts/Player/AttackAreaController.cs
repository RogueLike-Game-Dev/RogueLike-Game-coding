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
            Debug.Log(collision.gameObject.name + " Current Hp: " + collisionStats.currentHP);
        }
        if (collision.rigidbody != null) //If enemy has rigidbody, push it back
        {
            Debug.Log("Attack area found a rigidbody");// Calculate Angle Between the collision point and the player
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 playerPosition = transform.position;
            Vector2 dir = contactPoint.point - playerPosition;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            collision.rigidbody.velocity = Vector2.zero;
            collision.rigidbody.inertia = 0;
            collision.rigidbody.AddForce(dir *playerStats.knockBackStrength, ForceMode2D.Impulse);
        }

    }
}
