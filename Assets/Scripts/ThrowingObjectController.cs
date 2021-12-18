using UnityEngine;

public class ThrowingObjectController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
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
            
            enemyStats.Damage(damage);
            Destroy(this);
            Debug.Log(collision.gameObject.name + " Current Hp: " + enemyStats.currentHP);
        }
    }
}
