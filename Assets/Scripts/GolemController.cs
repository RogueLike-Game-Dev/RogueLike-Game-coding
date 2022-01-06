using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using System;

public class GolemController : MonoBehaviour
{
    private enum State
    {
        Walking,
        Running,
        Standing,
        Hurt,
        Dead
    }
    
    private State currentState;
    private GameObject player;
    private double rightEdge, leftEdge; 
    private EntityStats golemStats;
    
    private Animator animator;
    private Rigidbody2D rigidBody2D;
    private bool groundDetected, wallDetected;
    private bool flipWaiting = false;
    private bool exitStandingState, needFlip;
    
    private int facingDirection;
    
    private bool attackCooldown = false;
    
    [SerializeField] private GameObject attackArea;
    private EntityStats playerStats;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        golemStats = GetComponent<EntityStats>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        rightEdge = Math.Round(transform.parent.Find("RightEdge").transform.position.x);
        leftEdge = Math.Round(transform.parent.Find("LeftEdge").transform.position.x);
        Debug.Log(transform.parent.gameObject);
        currentState = State.Walking;
        facingDirection = 1;
        attackArea.SetActive(false);

        if (player)
        {
            playerStats = player.GetComponent<EntityStats>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (currentState)
        {
            case State.Walking:
                UpdateWalkingState();
                break;
            case State.Running:
                UpdateRunningState();
                break;
            case State.Standing:
                UpdateStandingState();
                break;
            default:
                break;
        }
    }
    
    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Walking:
                rigidBody2D.velocity = new Vector2(facingDirection*golemStats.movementSpeed, 0);
                break;
            case State.Running:
                rigidBody2D.velocity = new Vector2(facingDirection*4*golemStats.movementSpeed, 0);
                break;
			default:
				 rigidBody2D.velocity = new Vector2(0,rigidBody2D.velocity.y); 
				break;
        }

    }
    
    // Walking State

    private void EnterWalkingState()
    {
        animator.SetBool("Walking", true);
    }
    
    private void UpdateWalkingState()
    {
        if ((transform.position.x <= leftEdge && facingDirection<0) || (transform.position.x >= rightEdge  && facingDirection>0))
        {
            Flip();
        }

        if (!playerStats.isInvisible && !GameManager.isDying && player.transform.position.x > leftEdge && player.transform.position.x < rightEdge)
        {
           SwitchState(State.Running);
        }
    }
    
    private void ExitWalkingState()
    {
        animator.SetBool("Walking", false);
    }
    
    // Running State

    private void EnterRunningState()
    {
        animator.SetBool("Running", true);
    }
    
    private void UpdateRunningState()
    {
        if (!playerStats.isInvisible && !GameManager.isDying && (
            (player.transform.position.x < transform.position.x && facingDirection > 0) ||
            (player.transform.position.x > transform.position.x && facingDirection < 0)))
        {
            Flip();
        }

        if (!(player.transform.position.x > leftEdge && player.transform.position.x < rightEdge) || playerStats.isInvisible || GameManager.isDying)
        {
            SwitchState(State.Walking);
        }
		else if (!playerStats.isInvisible && !GameManager.isDying && transform.position.x - player.transform.position.x < 1.65 && transform.position.x - player.transform.position.x > -1.65)
		{
			SwitchState(State.Standing);
		}
    }
    
    private void ExitRunningState()
    {
        animator.SetBool("Running", false);
    }
    
    // Standing State

    private void EnterStandingState()
    {
        animator.SetBool("Standing", true);
    }
    
    private void UpdateStandingState()
    {
        if (playerStats.isInvisible || GameManager.isDying)
        {
            SwitchState(State.Walking);
        }
        else if (!playerStats.isInvisible && !GameManager.isDying && !(transform.position.x - player.transform.position.x < 1.75 && transform.position.x - player.transform.position.x > -1.75))
		{
			SwitchState(State.Running);
		}
        else if (!playerStats.isInvisible && !GameManager.isDying && (
            (player.transform.position.x < transform.position.x && facingDirection > 0) ||
            (player.transform.position.x > transform.position.x && facingDirection < 0)))
        {
            needFlip = true;
            Flip();
        }
        else
        {
            needFlip = false;
            if (!playerStats.isInvisible && !GameManager.isDying && player.transform.position.y < 0.5 + transform.position.y && player.transform.position.y > transform.position.y - 0.5)
                StartCoroutine(Attack());
        }
    }
    
    private void ExitStandingState()
    {
        animator.SetBool("Standing", false);
        exitStandingState = true;
    }

    // Dead State

    private void EnterDeadState()
    {
        currentState = State.Dead;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
    
    // other

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Walking:
                ExitWalkingState();
                break;
            case State.Running:
                ExitRunningState();
                break;
            case State.Standing:
                ExitStandingState();
                break;
        }
        
        switch (state)
        {
            case State.Walking:
                EnterWalkingState();
                break;
            case State.Running:
                EnterRunningState();
                break;
            case State.Standing:
                EnterStandingState();
                break;
        }

        currentState = state;
    }
    
    private IEnumerator Attack()
    {
        if (!attackCooldown)
        {
            Debug.Log("Golem Attacking");
            attackArea.SetActive(true);

            animator.SetTrigger("isAttacking");
            attackCooldown = true;
            yield return new WaitForSeconds(0.3f);
            attackArea.SetActive(false);
            yield return new WaitForSeconds(golemStats.timeBetweenAttacks);
            attackCooldown = false;
        }
    }

    private async void Flip()
    {
        if (!flipWaiting && currentState == State.Standing) //if enemy is in standing state delay the flip
        {
            flipWaiting = true;
            exitStandingState = false;
            await Task.Delay(1200);
            if (needFlip) //  flip only if enemy didn't exit standing state
            {
                facingDirection *= -1;
                transform.Rotate(0.0f, 180.0f, 0.0f);
            }

            flipWaiting = false;
        }
        else if (currentState != State.Standing) //if enemy is not in standing state flip instanly
        {
            facingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }
}
