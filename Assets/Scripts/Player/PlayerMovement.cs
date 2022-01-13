using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region movementVariables
    [SerializeField] public float jumpForce = 5f;
    [SerializeField] private float  acceleratedFallSpeed = 0.15f;
    [SerializeField] private float deltaVelocityXDecay = 15;
    [SerializeField] private const float knockBackDuration = 0.5f;
    private float moveSpeed;
    [SerializeField] private float dashForce = 8f;
    [Range(0.1f, 1f)] [SerializeField] private float dashCooldown = 0.5f;
    [Range(1, 3)] public int maxJumps = 2;
    [Range(1, 3)] public int maxDashes = 2;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private Transform feetPosition;
	[SerializeField] private GameObject lightningEffect; 
    SpriteRenderer spriteRenderer;
    private Color tempColor;
    #endregion

    #region auxVariables

    private Rigidbody2D rigidBody2D;
	private bool doubleCoins;

    private bool doubleHeal;
    private Animator animator;
    [HideInInspector] public bool facingRight = true;
    private bool isDashing;
    private bool isKnockedback;
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
    private GameObject bubbleShield;
    public bool bubbleShieldActive;
    public GameObject[] zhaxThrowingObjects = new GameObject[7];
    private readonly float[] torques = {25, 15, 30, 20, 20, 10, 20};
    private int throwingForce;
    private const float throwingCooldownTime = 10.0f;
    private bool throwingCooldown;

	private const float lightningCooldownTime = 10.0f;
    private bool lightningCooldown;

    private string fallingTriggerKey = "isFalling";
    private string movingBoolKey = "isMoving";
    private string throwingTriggerKey = "isThrowing";
    private string attackingTriggerKey = "isAttacking";
    private string jumpingTriggerKey = "isJumping";
    private string groundedBoolKey = "isGrounded";

    private PurchasedItems purchasedItems;
    private const float speedIncrease = 1.6f;
    private const int damageIncrease = 10;
    private const int hpIncrease = 200;
    private const int armorIncrease = 100;
    private const int hpRegenIncrease = 5;

    private bool canRegenHp = true;
    private bool resetAnimation;
    public Vector3 initialPosition;

    public enum CharacterType
    {
        Zhax,       // Diana's player
        Demetria,   // Radu's player
        Esteros,    // Paula's player
        Lyn,		// Vlad's player
    }

    public static CharacterType characterType;

    private Dictionary<string, KeyCode> keybindDict;
    
    #endregion
    
    // Start is called before the first frame update
    void Start()
    { 
        //Get references
        playerStats = GetComponent<EntityStats>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        playerStats.gold = RunStats.goldCollected;
        playerStats.enemiesKilled = RunStats.enemiesKilled;
       
        
        purchasedItems = PurchasedItems.getInstance();
        
        // manage armor
        if (purchasedItems.armorMaxLevel >= 3)
        {
            playerStats.maxArmor += 3 * armorIncrease;
        }
        else if (purchasedItems.armorMaxLevel >= 2)
        {
            playerStats.maxArmor += 2 * armorIncrease;
        }
        else if (purchasedItems.armorMaxLevel >= 1)
        {
            playerStats.maxArmor += armorIncrease;
        }

        playerStats.currentArmor = playerStats.maxArmor;
        
        // manage hp
        if (purchasedItems.hpMaxLevel >= 4)
        {
            playerStats.maxHP += 3 * hpIncrease;
        }
        else if (purchasedItems.hpMaxLevel >= 2)
        {
            playerStats.maxHP += 2 * hpIncrease;
        }
        else if (purchasedItems.hpMaxLevel >= 1)
        {
            playerStats.maxHP += hpIncrease;
        }
        
        InitialValues.remainingHP = playerStats.maxHP; // remove this line when InitialValues.remainingHP is calculated for the first time

        // manage hp regen
        if (purchasedItems.hpMaxLevel >= 6)
        {
            playerStats.hpRegen = 3 * hpRegenIncrease;
        }
        else if (purchasedItems.hpMaxLevel >= 5)
        {
            playerStats.hpRegen = 2 * hpRegenIncrease;
        }
        else if (purchasedItems.hpMaxLevel >= 3)
        {
            playerStats.hpRegen = hpRegenIncrease;
        }

        // manage damage
        if (purchasedItems.damageMaxLevel >= 4)
        {
            playerStats.DMG += 3 * damageIncrease;
        }
        else if (purchasedItems.damageMaxLevel >= 2)
        {
            playerStats.DMG += 2 * damageIncrease;
        }
        else if (purchasedItems.damageMaxLevel >= 1)
        {
            playerStats.DMG += damageIncrease;
        }
        
        // manage speed and triple jump
        if (purchasedItems.speedMaxLevel >= 3)
        {
            maxJumps = 3;
        }
        
        if (purchasedItems.speedMaxLevel >= 4)
        {
            playerStats.movementSpeed += 2.5f * speedIncrease;
        }
        else if (purchasedItems.speedMaxLevel >= 2)
        {
            playerStats.movementSpeed += 1.6f * speedIncrease;
        }
        else if (purchasedItems.speedMaxLevel >= 1)
        {
            playerStats.movementSpeed += speedIncrease;
        }
        
        moveSpeed = playerStats.movementSpeed;

        if (characterType.Equals( CharacterType.Esteros))
        {
            bubbleShield = GameObject.Find("BubbleShield");
            bubbleShield.SetActive(false);
            bubbleShieldActive = false;
        }
        
        if (!attackArea)
        {
            attackArea = GameObject.Find("AttackArea");
        }
        
        attackArea.SetActive(false);
        tempColor = spriteRenderer.color;

        initialPosition = transform.position;
        
        print("CHARACTER TYPE: " + characterType);

        //manage keybinds
        keybindDict = new Dictionary<string, KeyCode>();

        keybindDict.Add("Up", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up", "W")));
        keybindDict.Add("Left", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A")));
        keybindDict.Add("Down", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down", "S")));
        keybindDict.Add("Right", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D")));
        keybindDict.Add("Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Attack", "Mouse0")));
        keybindDict.Add("Special Attack", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Special Attack", "Mouse1")));
        keybindDict.Add("Dash", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Dash", "E")));
        keybindDict.Add("Interact", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "G")));
        keybindDict.Add("Item1", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Item1", "Alpha1")));
        keybindDict.Add("Item2", (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Item2", "Alpha2")));
    }
    
    private void Update()
    {
        if (GameManager.isDying)
        {
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<Collider2D>().enabled = false;
            return;
        }
        
        if (GameManager.wasRevived && !resetAnimation)
        {
            animator.Play("Idle");
            GetComponent<Collider2D>().enabled = true;
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            resetAnimation = true;
        }

        if (playerStats.currentHP < playerStats.maxHP && canRegenHp)
        {
            StartCoroutine(HpRegen());
        }
        
        //Input handling in Update, force handling in FixedUpdate 
      
        
        moveDirection = Input.GetAxis("Horizontal");
        if (moveDirection > 0 && !facingRight)
            Flip();
        else if (moveDirection < 0 && facingRight)
            Flip();
        if (moveDirection != 0)
        {
            animator.SetBool(movingBoolKey, true);
            isMoving = true;
        }
        else
        {
            animator.SetBool(movingBoolKey, false);
            isMoving = false;
        }

        if (Input.GetKeyDown(keybindDict["Special Attack"]))
            SpecialAttack();
        if (Input.GetKeyDown(keybindDict["Attack"]))
            StartCoroutine(Attack());

        if (Input.GetKeyDown(keybindDict["Up"]))
        {
            Jump();
        }

        if (Input.GetKeyDown(keybindDict["Dash"]))
        {
            if (Input.GetKey(keybindDict["Down"]))
                Stomp();
            else
            {
                if (!isDashing && isMoving)
                    StartCoroutine(Dash());
                else
                    Debug.Log("Dash on cooldown");
            }
        }
        
        if (isOnCollectible && Input.GetKeyDown(keybindDict["Interact"])) 
        {
            Destroy(collectible);
            playerStats.collectibles++;
        }
        
        if (rigidBody2D.velocity.y < 0)
        {
            animator.SetTrigger(fallingTriggerKey);
            if (transform.position.y <= -60.0 && !GameManager.isDying)
            {
                animator.SetTrigger("isDying");
                GameManager.EndRun();
            }
        }
        
        // check if player has Immunity item bought
        // if yes, then check if the user activates it (presses a numeric key)
        // then playerStats.isInvulnerable = true and decrement the number of immunity items
        if (purchasedItems.immunityNr > 0)
        {
            if (Input.GetKeyDown(keybindDict["Item1"]) && !playerStats.isInvulnerable)
            {
                purchasedItems.immunityNr--;
                StartCoroutine(WaitForImmunity());
            }
        }

        // idem immunity
        if (purchasedItems.invisibilityNr > 0)
        {
            if (Input.GetKeyDown(keybindDict["Item2"]) && !playerStats.isInvisible)
            {
                purchasedItems.invisibilityNr--;
                StartCoroutine(WaitForInvisibility());
            }
        }

        if (GameManager.makeInvulnerableAfterRevive)
        {
            StartCoroutine(WaitForImmunity());
            GameManager.makeInvulnerableAfterRevive = false;
        }
    }

    private IEnumerator HpRegen()
    {
        if (playerStats.currentHP > 0)
        {
            playerStats.Heal(playerStats.hpRegen);
            canRegenHp = false;
            yield return new WaitForSeconds(1.0f);
            canRegenHp = true;
        }
    }

    void FixedUpdate()
    {
        if (!isDashing && !isKnockedback)
        {
            // Velocity when key is pressed
            if (moveDirection != 0)
            {
                rigidBody2D.velocity = new Vector2(moveSpeed * moveDirection, rigidBody2D.velocity.y);
                moveDirection = 0;
            }

            // Velocity when falling
            else if (!isGrounded && rigidBody2D.velocity.y < 2)
            {
                var velocity = rigidBody2D.velocity;
                rigidBody2D.velocity = new Vector2(
                    velocity.x - velocity.x / deltaVelocityXDecay, 
                    velocity.y - acceleratedFallSpeed
                    );
            }
        }
    }

    #region Action Functions

    private void SpecialAttack()
    {
        if (characterType.Equals(CharacterType.Demetria))
        {
            StartCoroutine(Throw());
        }
        else if (characterType.Equals(CharacterType.Esteros))
        {
            StartCoroutine(AttackEsteros());
        }
        else if (characterType.Equals(CharacterType.Zhax))
        {
            StartCoroutine(AttackZhax());
        }
		else if (characterType.Equals(CharacterType.Lyn))
        {
            StartCoroutine(SpecialAbilityLyn());
        }
    }
    private IEnumerator Throw()
    {
        if(!attackCooldown)
        {
            Debug.Log("Throwing");
            var throwingObj = ObjectPooler.Instance.GetPooledObject("Throw");
            
            if(facingRight)
                throwingObj.transform.position = this.transform.position + Vector3.right;
            else
                throwingObj.transform.position = this.transform.position + Vector3.left;
            throwingObj.SetActive(true);
            animator.SetTrigger(throwingTriggerKey);
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
            attackArea.GetComponent<AttackAreaController>().SetFirstAttack(true);

            animator.SetTrigger(attackingTriggerKey);
            attackCooldown = true;
            yield return new WaitForSeconds(0.3f);
            attackArea.SetActive(false);
            attackCooldown = false;
        }
        else
            Debug.Log("Attack on cooldown");
    }

    private IEnumerator AttackEsteros()
    {
        playerStats.isInvulnerable = true;
        bubbleShield.SetActive(true);
        bubbleShieldActive = true;
        yield return new WaitForSeconds(3f);
        bubbleShield.SetActive(false);
        bubbleShieldActive = false;
        playerStats.isInvulnerable = false;
    }

    private IEnumerator AttackZhax()
    {
        if (!throwingCooldown)
        {
            if (jumpCount != 0 || isMoving)
            {
                throwingForce = 10;
            }
            else
            {
                throwingForce = 5;
            }
            var objectIndex = Random.Range(0, zhaxThrowingObjects.Length);
            var throwingObject = Instantiate(zhaxThrowingObjects[objectIndex]);
            throwingObject.transform.position = transform.position;
            throwingObject.layer = 8;

            var minus = facingRight ? 1 : -1;
            var rigidBodyObject = throwingObject.GetComponent<Rigidbody2D>();
            var throwDirection = new Vector2(minus * throwingForce, 5);

            rigidBodyObject.AddForce(throwDirection, ForceMode2D.Impulse);
            rigidBodyObject.AddTorque(torques[objectIndex]);

            throwingCooldown = true;
            yield return new WaitForSeconds(throwingCooldownTime);
            throwingCooldown = false;

            Destroy(throwingObject);
        }
        else
        {
            print("Throwing on cooldown");
        }
    }

  private IEnumerator SpecialAbilityLyn()
    {
        if (!lightningCooldown)
        {
            
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			bool ok = false;		 
			foreach (GameObject enemy in enemies)
            {
                   if (isNear(gameObject, enemy))
                   {
                        	Debug.Log(enemy.name + " is struck by lightning");
                        	Instantiate(lightningEffect, enemy.transform.position, enemy.transform.rotation).transform.SetParent(enemy.transform);
                        	enemy.GetComponent<EntityStats>().Damage(2*playerStats.DMG);
							ok = true;
                        
                    }
             } 
			if (!ok)
			{	
			 	if (!isKnockedback && !isDashing)
				{
				
				
					rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
            		rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            		animator.SetTrigger(jumpingTriggerKey);
            		isGrounded = false;
            		animator.SetBool(groundedBoolKey, isGrounded);
				
				}
				else 
				{
					yield return null;
				}
			}

            lightningCooldown = true;
            yield return new WaitForSeconds(lightningCooldownTime);
            lightningCooldown = false;
            
        }
        else
        {
            print("Special ability on cooldown");
        }
    }

	

    private void Jump()
    {
        if (isKnockedback || isDashing)
            return;
        if (isGrounded || jumpCount < maxJumps)
        {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
            rigidBody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetTrigger(jumpingTriggerKey);
            isGrounded = false;
            animator.SetBool(groundedBoolKey, isGrounded);

            jumpCount++;
        }
    }
    private void Flip()
    {
        
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        var negative = 1;
        if (!facingRight)
        {
            negative = -1;
        }
        
        spriteRenderer.flipX =! spriteRenderer.flipX;
        
        var attackAreaSize = attackArea.GetComponent<BoxCollider2D>().size;
        var playerColliderSizeX = GetComponent<BoxCollider2D>().size.x;

        var attackLocalPosition = attackArea.transform.localPosition;
        var newX = attackLocalPosition.x + negative * (attackAreaSize.x + playerColliderSizeX);
        var newY = attackLocalPosition.y;
        var newZ = attackLocalPosition.z;
        
        attackArea.transform.localPosition = new Vector3(newX, newY, newZ);
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
    private IEnumerator KnockBack(Vector2 dir, float knockBackStrength, float duration = knockBackDuration)
    {
        isKnockedback = true;
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.inertia = 0;
        rigidBody2D.AddForce(dir * knockBackStrength, ForceMode2D.Impulse);

        yield return new WaitForSeconds(duration);

        isKnockedback = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionStats = collision.gameObject.GetComponent<EntityStats>(); //Daca e colision cu ceva care da DMG 

        // Calculate Angle Between the collision point and the player
        ContactPoint2D contactPoint = collision.GetContact(0);
        Vector2 playerPosition = transform.position;
        Vector2 dir = contactPoint.point - playerPosition;
        
        if (collisionStats != null && playerStats.isInvulnerable)
        {
            if (characterType.Equals(CharacterType.Esteros) && bubbleShieldActive)
            {
                if (!collision.gameObject.CompareTag("Enemy"))
                    collision.gameObject.SetActive(false);
            }
        }
        else if (collisionStats != null)
        {
            
                playerStats.Damage(collisionStats.DMG);
            

            // We get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;

            // knockBack player (knockBack on Y works but not on X)
            StartCoroutine(KnockBack(dir, playerStats.knockBackStrength));
        }

        // Check relative direction on Y axis to see if impact ocurred between map and the bottom of the player
        // The -0.85 value is hardcoded and should be changed along with the player's collision box 
        if (collision.gameObject.name == "Tilemap" || collision.gameObject.CompareTag("Ground")) 
        {
                if (dir.y < 0)
                {
                    jumpCount = 0;
                    isGrounded = true;
                    animator.SetBool(groundedBoolKey, isGrounded); 
                }
                // Add a very small amount of knockBack on collision with walls.
                else
                    StartCoroutine(KnockBack(dir, 2f, 0.2f));
                
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
			if (doubleCoins)
			{
				RunStats.goldCollected++;
            	playerStats.gold++;
			}

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
            if (doubleHeal)
            {
                playerStats.Heal(2 * 5);
            }
            else 
            {
                playerStats.Heal(5); //Oare e o idee buna sa fie hard coded aici?
            }
            Debug.Log("Restored HP");
        }
        else if (collision.gameObject.CompareTag("Heart")) //Picked up a Heart
        {
            collision.gameObject.SetActive(false);
            if (doubleHeal)
            {
                playerStats.Heal(2 * 10);
            }
            else 
            {
                playerStats.Heal(10); 
                Debug.Log("Restored 10 HP");
            
            }
            
        }
        else if (collision.gameObject.CompareTag("Star")) //Picked up a star
        {
            collision.gameObject.SetActive(false);
            if (playerStats.currentHP != playerStats.maxHP)
            {
                if (doubleHeal)
                {
                    playerStats.Heal(2 * 15);
                }
                else 
                {
                    playerStats.Heal(15); 
                    Debug.Log("Restored 15 HP");
            
                }
            }
            else
            {
                
                if (doubleHeal)
                {
                    playerStats.maxHP += 2 * 10;
                }
                else 
                {
                    playerStats.maxHP += 10;
                    
                }
                Debug.Log("Maximised HP");
            }
        }
        else if (collision.gameObject.CompareTag("Lava"))
        {
            playerStats.currentArmor = 0;   // the armor melts and then the player dies
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

    private void SetActiveChildCollision(Collider2D collision, int childNumber, bool active)
    {
        var parent = collision.transform.parent; 
        if (parent)
        {
            if (parent.childCount > childNumber)
            {
                var child = parent.gameObject.transform.GetChild(childNumber);
                child.gameObject.SetActive(active);
            }
        }
    }
	public void SetDoubleCoins(bool val)
	{
		doubleCoins = val;
	}
    
    public void SetDoubleHeal(bool val)
    {
        doubleHeal = val;
    }

	private bool isNear(GameObject player, GameObject ob)
    {
        return ((player.transform.position.x + 7 > ob.transform.position.x) &&
                (player.transform.position.x - 7 < ob.transform.position.x) &&
                (player.transform.position.y + 7 > ob.transform.position.y) &&
                (player.transform.position.y - 7 < ob.transform.position.y));
    }

    private IEnumerator WaitForImmunity()
    {
        playerStats.isInvulnerable = true;
        tempColor = new Color(1, 0.92f, 0, spriteRenderer.color.a);
        spriteRenderer.color = tempColor;
        yield return new WaitForSeconds(5.0f);
        tempColor = new Color(1, 1, 1, spriteRenderer.color.a);
        spriteRenderer.color = tempColor;
        playerStats.isInvulnerable = false;
    }

    private IEnumerator WaitForInvisibility()
    {
        playerStats.isInvisible = true;
        tempColor.a = 0.4f;
        spriteRenderer.color = tempColor; 
        yield return new WaitForSeconds(5.0f);
        tempColor.a = 1.0f;
        spriteRenderer.color = tempColor; 
        playerStats.isInvisible = false;
    }
}
