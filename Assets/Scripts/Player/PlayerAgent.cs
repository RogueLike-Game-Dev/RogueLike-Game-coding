using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Unity.MLAgents.Actuators;
using System.Collections;

public class PlayerAgent : Agent
{
    #region PlayerStats
    [SerializeField] private float jumpForce = 20f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float dashForce = 40f;
    [SerializeField] private float dashCooldown = 0.5f;
    public int maxJumps = 2;
    public int maxDashes = 2;
    #endregion
    #region auxVar
    private Rigidbody2D rigidBody2D;
    private Animator animator;
    private bool facingRight = true;


    private bool isDashing = false;
    private bool isGrounded = true;
    private float moveDirection = 0f;
    private int jumpCount = 0;
    #endregion
    public Transform target;
    public Transform target2;
    public Transform target3;
    public Transform target4;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum
        if (transform.localPosition.y < -5)
        {
            rigidBody2D.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0, 0.5f, 0);
        }

        // Move the target to a new spot
        var choice = Random.value;
        if (choice < 0.24)
            target.localPosition = new Vector2(Random.value * 8 - 4, -1f);
        else if (choice < 0.5f)
            target.localPosition = target2.localPosition;
        else if (choice < 0.75f)
            target.localPosition = target3.localPosition;
        else
            target.localPosition = target4.localPosition;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rigidBody2D.velocity.x);
        sensor.AddObservation(rigidBody2D.velocity.y);

        //Jump and dashing
        sensor.AddObservation(jumpCount);
        sensor.AddObservation(isDashing);

        //Positions: 6 floats
        //Velocity: 2 floats
        //Jump & Dash: 1 int, 1 bool
        //Total: 10 values
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        // Actions, size = 2
        // 0 -> Input.GetAxis("Horizontal")
        // 1 -> 1 daca sare, 0 altfel
        // 2 -> dashing
        moveDirection = actions.ContinuousActions[0];

        if (moveDirection > 0 && !facingRight)
            Flip();
        else if (moveDirection < 0 && facingRight)
            Flip();
        if (moveDirection != 0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);

        if (actions.ContinuousActions[1] == 1f)
            Jump();

        if (actions.ContinuousActions[2]==1f)
            if (!isDashing)
                StartCoroutine(Dash());
        if (rigidBody2D.velocity.y < 0)
            animator.SetTrigger("isFalling");
        if (!isDashing)
            rigidBody2D.velocity = new Vector2(moveSpeed * moveDirection, rigidBody2D.velocity.y);

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, target.localPosition);

        // Reached target
        if (distanceToTarget < 0.42f)
        {
            SetReward(1.0f);
            Debug.Log("Found reward");
            EndEpisode();
        }
        // Fell off platform
        else if (this.transform.localPosition.y < -5)
        {
            SetReward(-1f);
            EndEpisode();
            Debug.Log("Fell");
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) ? 1.0f : 0.0f;
        continuousActionsOut[2] = Input.GetKey(KeyCode.E) ? 1.0f : 0.0f;
    }

    #region Control
    private void Jump()
    { 
        if (isGrounded || jumpCount < maxJumps)
        {
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
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, 0);
        if (facingRight)
            rigidBody2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        else
            rigidBody2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
        float gravity = rigidBody2D.gravityScale;
        rigidBody2D.gravityScale = 0;
        yield return new WaitForSeconds(0.4f);
        rigidBody2D.gravityScale = gravity;
        yield return new WaitForSeconds(dashCooldown); //Dash cooldown
        isDashing = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //TO DO: CHECK IF COLLISION IS GROUND 
     
        isGrounded = true;
        jumpCount = 0;
        animator.SetBool("isGrounded", isGrounded);
    }
    #endregion
}
