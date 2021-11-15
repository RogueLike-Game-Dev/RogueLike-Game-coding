using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region movementVariables
    [SerializeField] private float jumpForce = 5f;
    private float moveSpeed;
    [SerializeField] private float dashForce = 8f;
    [Range(0.1f, 1f)] [SerializeField] private float dashCooldown = 0.5f;
    [Range(1, 3)] public int maxJumps = 2;
    [Range(1, 3)] public int maxDashes = 5;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private Transform feetPosition;
    SpriteRenderer spriteRenderer;
    #endregion

    #region auxVariables
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    [HideInInspector] public bool facingRight = true;
    private bool isDashing;
    private bool isGrounded = true;
    private bool isMoving;
    private bool isOnCollectible;
    private bool attackCooldown;
    private float moveDirection;
    private int jumpCount;
    private int dashCount;
    private EntityStats playerStats;
    private GameObject collectible;
    private bool dialogueActive;

    #endregion
    // Start is called before the first frame update
    void Start()
    { //Get references
        playerStats = GetComponent<EntityStats>();
        moveSpeed = playerStats.movementSpeed;
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackArea.SetActive(false);

    }
    private void Update()
    {//Input handling in Update, force handling in FixedUpdate 
        
        moveDirection = Input.GetAxis("Horizontal");
        if (moveDirection > 0 && !facingRight)
            Flip();
        else if (moveDirection < 0 && facingRight)
            Flip();
        if (moveDirection != 0)
        {
            animator.SetBool("isMoving", true);
            isMoving = true;
        }
        else
        {
            animator.SetBool("isMoving", false);
            isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
            StartCoroutine(Throw());
        if (Input.GetKeyDown(KeyCode.Mouse0))
            StartCoroutine(Attack());
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            Jump();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                Stomp();
            else
            {
                if (!isDashing && isMoving)
                    StartCoroutine(Dash());
                else
                    Debug.Log("Dash on cooldown");
            }
        }
        
        if (isOnCollectible && Input.GetKeyDown(KeyCode.G)) 
        {
            Destroy(collectible);
            playerStats.collectibles++;
        }
        
        if (rigidBody2D.velocity.y < 0)
            animator.SetTrigger("isFalling");
    }

    void FixedUpdate()
    {
        if (!isDashing)
            rigidBody2D.velocity = new Vector2(moveSpeed * moveDirection, rigidBody2D.velocity.y);
    }

    #region Action Functions
    private IEnumerator Throw()
    {
        if(!attackCooldown)
        {
            Debug.Log("Throwing");
            var throwingObj = ObjectPooler.Instance.GetPooledObject("Throw");
            //throwingObj.SetDirection();
            if(facingRight)
                throwingObj.transform.position = this.transform.position + Vector3.right;
            else
                throwingObj.transform.position = this.transform.position + Vector3.left;
            throwingObj.SetActive(true);
            animator.SetTrigger("isThrowing");
            attackCooldown = true;
            yield return new WaitForSeconds(playerStats.timeBetweenAttacks);
           
            attackCooldown = false;
        }
    }
    private IEnumerator Attack()
    {
        if (!attackCooldown && !dialogueActive)
        {
            Debug.Log("Attacking");
            attackArea.SetActive(true);

            animator.SetTrigger("isAttacking");
            attackCooldown = true;
            yield return new WaitForSeconds(0.3f);
            attackArea.SetActive(false);
            attackCooldown = false;
        }
        else
            Debug.Log("Attack on cooldown");
    }
    private void Jump()
    {
        if (isGrounded || jumpCount < maxJumps)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.y, 0);
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger("isJumping");
            isGrounded = false;
            animator.SetBool("isGrounded", isGrounded);

            jumpCount++;
        }
    }
    private void Flip()
    {
        
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        // Multiply the player's x local scale by -1.
        // Vector3 theScale = transform.localScale;
        // theScale.x *= -1;
        // transform.localScale = theScale;
        spriteRenderer.flipX =! spriteRenderer.flipX;
    }
    private void Stomp()
    {
        if (!isGrounded)
        {
            rigidBody2D.velocity = new Vector2(0, 0);
            rigidBody2D.AddForce(2 * dashForce * Vector2.down, ForceMode2D.Impulse);
        }
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);

        if (facingRight)
            rigidBody2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        else
            rigidBody2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);

        //Remember gravity scale so we can set it back later
        float gravity = rigidBody2D.gravityScale;
        rigidBody2D.gravityScale = 0; //Null gravity to dash on horizontal
        dashCount++;
        yield return new WaitForSeconds(0.4f);
        rigidBody2D.gravityScale = gravity;
        if (dashCount < maxDashes)
        {
            isDashing = false;
            yield return null;
        }
        else
        {
            yield return new WaitForSeconds(dashCooldown); //Dash cooldown
            isDashing = false;
            dashCount = 0;
        }
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionStats = collision.gameObject.GetComponent<EntityStats>(); //Daca e colision cu ceva care da DMG 

        // Calculate Angle Between the collision point and the player
        ContactPoint2D contactPoint = collision.GetContact(0);
        Vector2 playerPosition = transform.position;
        Vector2 dir = contactPoint.point - playerPosition;

        if (collisionStats != null)
        { playerStats.Damage(collisionStats.DMG);

            //Knockback player (TODO)
            //We get the opposite (-Vector3) and normalize it
            
            dir = -dir.normalized;
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.inertia = 0;
            rigidBody2D.AddForce(dir * collisionStats.knockBackStrength, ForceMode2D.Impulse);

        }
        //Check relative direction on Y axis to see if impact ocurred between map and the bottom of the player
        if (collision.gameObject.name == "Tilemap" || collision.gameObject.CompareTag("Ground")) 
        {
                isGrounded = true;
                jumpCount = 0;
                animator.SetBool("isGrounded", isGrounded); 
        }
        else Debug.Log("Player collided with: "+collision.gameObject.name); 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")) //Picked up a coin
        {
            collision.gameObject.SetActive(false);
            RunStats.goldCollected++;
            playerStats.gold++;
            Debug.Log("Player currently has: " + playerStats.gold + " gold");
        }
        else if (collision.gameObject.CompareTag("Key"))    // Picked up a key 
        {
            collision.gameObject.SetActive(false);
            RunStats.keysCollected++;
            playerStats.keys++;
            Debug.Log("Player currently has: " + playerStats.keys + " keys");
        }
        else if (collision.gameObject.CompareTag("Chest"))  // Collided with a chest that requires a key
        {
            if (playerStats.keys >= 1) {
                var unlockedChest = Resources.Load<Sprite>("Sprites/Chest_02_Unlocked");
                RunStats.keysCollected--;
                playerStats.keys--;
                collision.gameObject.GetComponent<SpriteRenderer>().sprite = unlockedChest;
                SetActiveChildCollision(collision, 1, true);
                Destroy(collision.gameObject.GetComponent<BoxCollider2D>());
            }
            Debug.Log("Player currently has: " + playerStats.keys + " keys");
        }
        else if (collision.gameObject.CompareTag("Gold Chest")) //Picked up a gold chest
        {
            RunStats.goldCollected += 100;
            playerStats.gold += 100;
            var unlockedChest = Resources.Load<Sprite>("Sprites/Chest_01_Unlocked");
            collision.gameObject.GetComponent<SpriteRenderer>().sprite = unlockedChest;
            Destroy(collision.gameObject.GetComponent<BoxCollider2D>());
            Debug.Log("Player currently has: " + playerStats.gold + " gold");
        }
        else if (collision.gameObject.CompareTag("Apple")) //Picked up an apple
        {
            collision.gameObject.SetActive(false);
            playerStats.Heal(5); //Oare e o idee buna sa fie hard coded aici?
            Debug.Log("Restored HP");
        }
        else if (collision.gameObject.CompareTag("Heart")) //Picked up a Heart
        {
            collision.gameObject.SetActive(false);
            playerStats.Heal(10);
            Debug.Log("Restored 10 HP");
        }
        else if (collision.gameObject.CompareTag("Star")) //Picked up a star
        {
            collision.gameObject.SetActive(false);
            if (playerStats.currentHP != playerStats.maxHP)
            {
                playerStats.Heal(15);
                Debug.Log("Restored 15 HP");
            }
            else
            {
                playerStats.maxHP += 10;
                Debug.Log("Maximised HP");
            }
        }
        else if (collision.gameObject.CompareTag("Lava")) 
        {
            playerStats.Damage(playerStats.currentHP);
            Debug.Log("You have died!");
        }
        else if (collision.gameObject.CompareTag("Diamond")) 
        {
            collision.gameObject.SetActive(false);
            RunStats.goldCollected += 250;
            playerStats.gold += 250;  
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            SetActiveChildCollision(collision, 1, true);
            dialogueActive = true;
        }
        Debug.Log("Played entered trigger from: " + collision.gameObject.name);
    }

    private void OnTriggerStay2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Collectible")) 
        {
            isOnCollectible = true;
            collectible = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectible"))
        {
            isOnCollectible = false;
            collectible = null;
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            SetActiveChildCollision(collision, 1, false);
            dialogueActive = false;
        }
    }

    private void SetActiveChildCollision(Collider2D collider, int childNumber, bool active)
    {
        var parent = collider.transform.parent; 
        if (parent)
        {
            if (parent.childCount > childNumber)
            {
                var child = parent.gameObject.transform.GetChild(childNumber);
                child.gameObject.SetActive(active);
            }
        }
    }
}
