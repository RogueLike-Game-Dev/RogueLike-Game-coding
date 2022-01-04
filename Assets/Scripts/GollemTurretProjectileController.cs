using System.Collections;
using UnityEngine;

public class GollemTurretProjectileController : MonoBehaviour
{

    public int damage = 10;
    public int bounceTimes = 1;
    public float lifeTime = -1; //-1 for infinite
    private int hasBounced = 0;
    private Rigidbody2D rigidBody2D;
    private Animator animator;

    void Start() 
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (animator != null) //resize and reorient the animation, if it exists
        {
            //TODO: get orientation flags from parent (might need to change Instantiate to make the projectile a child of the turret)
            var scale = transform.localScale;
            scale.x *= -0.05f;
            scale.y *= -0.05f;
            transform.localScale = scale;
        }
        if (lifeTime != -1)
        {
            StartCoroutine(DestroyAfterTime(lifeTime));
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasBounced == bounceTimes) 
        {
            Destroy(this.gameObject);
        }
        hasBounced++;
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DamagePlayer(collision));
        }
    }
    
    private IEnumerator DamagePlayer(Collision2D collision)
    {
        var playerStats = collision.gameObject.GetComponent<EntityStats>();
            
        playerStats.Damage(damage);

        yield return new WaitForSeconds(0.5f);

    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);

    }
}
