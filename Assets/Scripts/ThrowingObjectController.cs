using System.Collections;
using UnityEngine;

public class ThrowingObjectController : MonoBehaviour
{
    private bool hitGround;
    private PurchasedItems purchasedItems;
    private int damageIncrease;

    private void Start()
    {
        purchasedItems = PurchasedItems.getInstance();
        if (purchasedItems.damageMaxLevel >= 5) // second level of magical damage
        {
            damageIncrease = 4;
        }
        else if (purchasedItems.damageMaxLevel >= 3)    // first level of magical damage
        {
            damageIncrease = 2;
        }
        else
        {
            damageIncrease = 0;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hitGround)
        {
            StartCoroutine(DamageEnemy(collision));
        }
        else
        {
            hitGround = true;
        }
    }
    
    private IEnumerator DamageEnemy(Collision2D collision)
    {
        var enemyStats = collision.gameObject.GetComponent<EntityStats>();
        print(name);

        int damage;
        if (name.Contains("Rock"))
        {
            damage = 5;
        }
        else if (name.Contains("Knife"))
        {
            damage = 10;
        }
        else
        {
            damage = 2;
        }
        
        damage += damageIncrease;
            
        enemyStats.Damage(damage);

        yield return new WaitForSeconds(0.5f);
        
        Destroy(gameObject);
        Debug.Log(collision.gameObject.name + " Current Hp: " + enemyStats.currentHP);
    }
}
