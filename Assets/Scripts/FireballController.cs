using UnityEngine;

public class FireballController : MonoBehaviour
{
    private EntityStats fireballStats;
    private bool facingRight;
    private bool flipped = false;
    private string caster;
    private void Start()
    {
        
        fireballStats = GetComponent<EntityStats>();
    }
    public void ResetValues(bool facingRight, string whoCasted)
    {
        flipped = false;
        this.facingRight = facingRight;
        caster = whoCasted;
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
   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.name!= caster)
        {
            var collisionStats = collision.gameObject.GetComponent<EntityStats>();
            if (collisionStats != null)
            {
                collisionStats.Damage(fireballStats.DMG);
                Debug.Log("Fireball collided with entity " + collision.gameObject.name + " Current Hp: " + collisionStats.currentHP);
            }
            if (collision.attachedRigidbody != null) //If enemy has rigidbody, push it back
            {
                Debug.Log("Fireball found a rigidbody");// Calculate Angle Between the collision point and the player
                ContactPoint2D[] contactPoints = new ContactPoint2D[10];
                collision.GetContacts(contactPoints);
                Vector2 playerPosition = transform.position;
                Vector2 dir = contactPoints[0].point - playerPosition;
                // We then get the opposite (-Vector3) and normalize it
                dir = dir.normalized;
                collision.attachedRigidbody.velocity = Vector2.zero;
                collision.attachedRigidbody.inertia = 0;
                collision.attachedRigidbody.AddForce(dir * 5, ForceMode2D.Impulse);
            }
        }
    }
    public void OnAnimationFinish() //Called via Animation Event when it finishes
    {
        this.gameObject.SetActive(false); //Put object back into the pool
    }
}
