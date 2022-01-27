using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;

public class Catcher : Agent
{
    #region movementVariables
    [SerializeField] private float jumpForce = 2f;
    private float moveSpeed;
    [SerializeField] private float dashForce = 20f;
    [Range(0.1f, 1f)] [SerializeField] private float dashCooldown = 0.5f;
    [Range(1, 3)] public int maxJumps = 2;
    [Range(1, 3)] public int maxDashes = 2;
    [SerializeField] private Transform feetPosition;
    #endregion
    #region Training
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private TagEnvController tagEnvController;
    #endregion
    #region auxVariables
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    public bool facingRight = true;
    private bool isDashing = false;
    private bool isGrounded = true;
    private float moveDirection = 0f;
    private int jumpCount = 0;
    private int dashCount = 0;
    private EntityStats playerStats;
    private SpriteRenderer spriteRenderer;
    #endregion
    // Start is called before the first frame update
    void Start()
    { //Get references
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<EntityStats>();
        moveSpeed = playerStats.movementSpeed;
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); // 3 floats pt pozitia lui
        sensor.AddObservation(enemyTransform.localPosition); // 3 floats pt pozitia adversarului
        sensor.AddObservation(playerStats.currentHP); //1 val pt hp-ul curent

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.0003f); //Incentivize agents to end episode as quickly as possible
        var discreteActions = actions.DiscreteActions;
        moveDirection = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var jump = discreteActions[0]; // 0 = don't jump, 1 = jump
        var dash = discreteActions[1];
        var attack = discreteActions[2];
        var stomp = discreteActions[3];
        var secondaryAttack = discreteActions[4];
        if (jump == 1)
            Jump();
        if (dash == 1)
            StartCoroutine(Dash());
        if (stomp == 1)
            Stomp();

    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Wanting to jump");
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
            discreteActionsOut[2] = 1;
        if (Input.GetKeyDown(KeyCode.Mouse1))
            discreteActionsOut[4] = 1;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                discreteActionsOut[3] = 1;
            else
            {
                if (!isDashing)
                    discreteActionsOut[1] = 1;

            }
        }
    }
    private void Update()
    {//Input handling in Update, force handling in FixedUpdate 

        if (moveDirection > 0 && !facingRight)
            Flip();
        else if (moveDirection < 0 && facingRight)
            Flip();
        if (moveDirection != 0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
        if (rigidBody2D.velocity.y < 0)
            animator.SetTrigger("isFalling");
    }

    void FixedUpdate()
    {
        if (!isDashing)
            rigidBody2D.velocity = new Vector2(moveSpeed * moveDirection, rigidBody2D.velocity.y);
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

        facingRight = !facingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
    private void Stomp()
    {
        if (!isGrounded)
        {
            rigidBody2D.velocity = new Vector2(0, 0);
            rigidBody2D.AddForce(Vector2.down * dashForce * 2, ForceMode2D.Impulse);
        }
    }
    private IEnumerator Dash()
    {
        AddReward(1f);
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionStats = collision.gameObject.GetComponent<EntityStats>(); //Daca e colision cu ceva care da DMG 

        
        if (collisionStats != null)
        {
            playerStats.Damage(collisionStats.DMG);
            AddReward(-1f);

            //Knockback player
            // Calculate Angle Between the collision point and the player
            ContactPoint2D contactPoint = collision.GetContact(0);
            Vector2 playerPosition = transform.localPosition;
            Vector2 dir = contactPoint.point - playerPosition;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            rigidBody2D.velocity = Vector2.zero;
            rigidBody2D.inertia = 0;
            rigidBody2D.AddForce(dir * collisionStats.knockBackStrength, ForceMode2D.Impulse);

        }
        if (collision.gameObject.layer == 7) //TO DO: CHECK IF IT WAS ON FEET
        {
            isGrounded = true;
            jumpCount = 0;
            animator.SetBool("isGrounded", isGrounded);
        }
        else Debug.Log("Catcher collided with: " + collision.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin") //Picked up a coin
        {
            collision.gameObject.SetActive(false);
            RunStats.goldCollected++;
            playerStats.gold++;
            Debug.Log("Player currently has: " + playerStats.gold + " gold");
        }
        else if (collision.gameObject.tag == "Apple") //Picked up an apple
        {
            collision.gameObject.SetActive(false);
            playerStats.Heal(15); //Oare e o idee buna sa fie hard coded aici?
            Debug.Log("Restored HP");
            AddReward(0.5f);
        }
        Debug.Log(this.gameObject.name + " entered trigger from: " + collision.gameObject.name + "CurrentHP: " + playerStats.currentHP);
    }

}