using UnityEngine;

public class FireballController : MonoBehaviour
{
    private EntityStats playerStats;
    private EntityStats fireballStats;
    private bool facingRight;
    private bool flipped = false;
    private void Start()
    {
        fireballStats = GetComponent<EntityStats>();
        var player = GameObject.Find("Player");
        playerStats = player.GetComponent<EntityStats>();
        facingRight = player.GetComponent<PlayerMovement>().facingRight;
    }
    private void Update()
    {
        if (facingRight)
            transform.position = new Vector2(transform.position.x + fireballStats.movementSpeed * Time.deltaTime, transform.position.y);
        else
        {
            if (!flipped)
                Flip();
            transform.position = new Vector2(transform.position.x - fireballStats.movementSpeed * Time.deltaTime, transform.position.y);
        }
    }
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        flipped = true;
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        var collisionStats = collision.gameObject.GetComponent<EntityStats>();
        if (collisionStats != null)
        {
            collisionStats.Damage(playerStats.DMG);
            Debug.Log("Fireball collided with entity "+collision.gameObject.name + " Current Hp: " + collisionStats.currentHP);
        }
        if (collision.rigidbody != null) //If enemy has rigidbody, push it back
        {
            Debug.Log("Fireball found a rigidbody");// Calculate Angle Between the collision point and the player
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 playerPosition = transform.position;
            Vector2 dir = contactPoint.point - playerPosition;
            // We then get the opposite (-Vector3) and normalize it
            dir = dir.normalized;
            collision.rigidbody.velocity = Vector2.zero;
            collision.rigidbody.inertia = 0;
            collision.rigidbody.AddForce(dir * playerStats.knockBackStrength, ForceMode2D.Impulse);
        }

    }

    public void OnAnimationFinish() //Called via Animation Event when it finishes
    {
        this.gameObject.SetActive(false); //Put object back into the pool
    }
}
