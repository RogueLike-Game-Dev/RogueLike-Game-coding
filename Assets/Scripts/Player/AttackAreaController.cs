using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAreaController : MonoBehaviour
{
    [SerializeField] private EntityStats playerStats;
    [SerializeField] private BossAgent agent;
    private GameObject effect;
    private bool showEffect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
      
        var collisionStats = collision.gameObject.GetComponent<EntityStats>();
        if (collisionStats != null)
        {
            
            if (showEffect)
            {
                Instantiate(effect, transform.position + new Vector3(-0.5f,1,0), transform.rotation);//.transform.SetParent(transform);
            }
            collisionStats.Damage(playerStats.DMG);
            if (agent != null)
                agent.SetReward(0.1f);
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

    public void SetEffect(GameObject newEffect)
    {
        effect = newEffect;
    }

    public void SetShowEffect(bool val)
    {
        showEffect = val;
    }
    
    
}
