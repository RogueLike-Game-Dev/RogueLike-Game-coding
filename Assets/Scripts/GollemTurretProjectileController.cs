using System.Collections;
using UnityEngine;
public class GollemTurretProjectileController : MonoBehaviour
{
    public bool facingRightOrUp = true; 
    public bool invertXY = false;
    public int damage = 10;
    public int bounceTimes = 1; //-1 for inifinite bounces
    public float lifeTime = -1; //-1 for infinite life-time
    public bool ignoreEnemyCollisions = true;
    private int hasBounced = 0;
    private Rigidbody2D rigidBody2D;
    private Animator animator;

    void Start() 
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (animator != null) //resize and reorient the animation, if it exists
        {
            var scale = transform.localScale;
            //animation default is moving rightwards
            if (facingRightOrUp && invertXY) //moving upwards
            {
                transform.Rotate(new Vector3 (0f, 0f, 90f));
            }
            else if (!facingRightOrUp && !invertXY) //moving leftwards
            {
                scale.x *= -1;
            }
            else if (!facingRightOrUp && invertXY) //moving downwards
            {
                transform.Rotate(new Vector3 (0f, 0f, -90f));
            }
            transform.localScale = scale;
            //Debug.Log(transform.eulerAngles);
        }
        if (lifeTime != -1)
        {
            StartCoroutine(DestroyAfterTime(lifeTime));
        } 
        //ignore collisions with enemies 
        if (ignoreEnemyCollisions)
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Physics2D.IgnoreCollision(enemy.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            }  
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var playerStats = collision.gameObject.GetComponent<EntityStats>();
            playerStats.Damage(damage);
        }
        if (hasBounced == bounceTimes) 
        {
            Destroy(this.gameObject);
        }
        hasBounced++;
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }
}
