using System.Collections;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;

public class BossAgent : Agent
{
    #region movementVariables
    [SerializeField] private float jumpForce = 2f;
    private float moveSpeed;
    [SerializeField] private float dashForce = 20f;
    [Range(0.1f, 1f)] [SerializeField] private float dashCooldown = 0.5f;
    [Range(1, 3)] public int maxJumps = 2;
    [Range(1, 3)] public int maxDashes = 2;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private Transform feetPosition;
    #endregion
    #region Training
    [SerializeField] private TrainingEnvController envController;
    [SerializeField] private Transform enemyTransform;
    #endregion
    #region auxVariables
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    public bool facingRight = true;
    private bool isDashing = false;
    private bool isGrounded = true;
    private bool attackCooldown = false;
    private float moveDirection = 0f;
    private int jumpCount = 0;
    private int dashCount = 0;
    private bool isJumping = false;
    private bool dashValue = false;
    private bool isAttacking = false;
    private bool isStomping = false;
    private bool isStompingOrDashing = false;
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
        attackArea.SetActive(false);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition); // 3 floats pt pozitia lui
        sensor.AddObservation(enemyTransform.localPosition); // 3 floats pt pozitia adversarului
        sensor.AddObservation(playerStats.currentHP); //1 val pt hp-ul curent
      
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        AddReward(-0.003f); //Incentivize agents to end episode as quickly as possible
        var discreteActions = actions.DiscreteActions;
        moveDirection = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var  action = discreteActions[0]; // 0 = do nothing, 1 jump, 2 dash, 3 attack, 4 stomp
        Debug.Log("Move Direction: " + moveDirection + "Action: " + action);
       switch (action) {
            case 0: break;
            case 1: Jump(); break;
            case 2: StartCoroutine(Dash()); break;
            case 3: StartCoroutine(Attack()); break;
            case 4: Stomp(); break;
           
        }
      
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        if (isJumping)
        {
            discreteActionsOut[0] = 1; }
        if (isAttacking)
            discreteActionsOut[0] = 3;
        /** Branch 0 has a size of 5, which means we can assign to it values from 0 to 4. What is the meaning of 5 here?
        if (Input.GetKeyDown(KeyCode.Mouse1))
            discreteActionsOut[0] = 5;
        **/
        if (isStompingOrDashing)
        {
            if (isStomping)
                discreteActionsOut[0] = 4;
            else
            {
                if (!isDashing)
                    discreteActionsOut[0] = 2;
            }
        }
        Debug.Log("Heuristic - The discrete action is" + discreteActionsOut[0]);
    }
    private void Update()
    {//Input handling in Update, force handling in FixedUpdate 

        isJumping = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) ? true : false;
        isAttacking = Input.GetKey(KeyCode.Mouse0) ? true : false;
        isStomping = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? true : false;
        isStompingOrDashing = Input.GetKey(KeyCode.E) ? true : false;

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

    #region Action Functions
    private IEnumerator Throw()
    {
        if (!attackCooldown)
        {  
            Debug.Log("Throwing");
            var throwingObj = ObjectPooler.Instance.GetPooledObject("Throw");
            throwingObj.transform.localPosition = this.transform.localPosition;
            throwingObj.GetComponent<FireballController>().ResetValues(this.facingRight, this.gameObject.name);
            throwingObj.SetActive(true);
            animator.SetTrigger("isThrowing");
            attackCooldown = true;
            yield return new WaitForSeconds(playerStats.timeBetweenAttacks);

            attackCooldown = false;
        }
    }
    private IEnumerator Attack()
    {
        
        if (!attackCooldown)
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
      
        isDashing = true;
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);

        if (facingRight)
            rigidBody2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        else
            rigidBody2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);

        //Remember gravity scale so we can set it back later
       
        rigidBody2D.gravityScale = 0; //Null gravity to dash on horizontal
        dashCount++;
        yield return new WaitForSeconds(0.4f);
        rigidBody2D.gravityScale = 1;
        Debug.Log("Current agent dashes: " + dashCount + " having maximum " + maxDashes);
        if (dashCount < maxDashes)
        {
            Debug.Log("Current agent dashed!");
            isDashing = false;
            yield return null;
        }
        else
        {
            Debug.Log("Current agent dashes max!");
            yield return new WaitForSeconds(dashCooldown); //Dash cooldown
            isDashing = false;
            dashCount = 0;
        }
    }
    #endregion
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.layer == 7) //TO DO: CHECK IF IT WAS ON FEET
        {
            isGrounded = true;
            jumpCount = 0;
            animator.SetBool("isGrounded", isGrounded);
        }
        else Debug.Log("Player collided with: " + collision.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coin") //Picked up a  coin
        {
            collision.gameObject.SetActive(false);
            RunStats.goldCollected++;
            playerStats.gold++;
            Debug.Log("Player currently has: " + playerStats.gold + " gold");
        }
        else if (collision.gameObject.tag == "Apple") //Picked up an apple
        {
            collision.gameObject.SetActive(false);
            playerStats.pickedApple = true;
            playerStats.Heal(15); //Oare e o idee buna sa fie hard coded aici?
            Debug.Log("Restored HP");
        }
        else if (collision.gameObject.tag == "Wall")//collided with a wall
        {
            AddReward(-0.003f);
            
        }
        Debug.Log(this.gameObject.name + " entered trigger from: " + collision.gameObject.name + "CurrentHP: " + playerStats.currentHP);
    }

   
}
