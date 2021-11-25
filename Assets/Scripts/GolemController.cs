using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private bool isAttacking;
    private bool groundDetected, wallDetected;
    
    private int facingDirection;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        golemStats = GetComponent<EntityStats>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
        rightEdge = Math.Round(GameObject.Find("RightEdge").transform.position.x);
        leftEdge = Math.Round(GameObject.Find("LeftEdge").transform.position.x);
        Debug.Log(transform.parent.gameObject);
        isAttacking = false;
        currentState = State.Walking;
        facingDirection = 1;
        
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
            case State.Hurt:
                UpdateHurtState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
        }
    }
    
    void FixedUpdate()
    {
        switch (currentState)
        {
            case State.Walking:
                rigidBody2D.velocity = new Vector2(facingDirection*golemStats.movementSpeed, rigidBody2D.velocity.y);
                break;
            case State.Running:
                rigidBody2D.velocity = new Vector2(facingDirection*4*golemStats.movementSpeed, rigidBody2D.velocity.y);
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


        if (player.transform.position.x > leftEdge && player.transform.position.x < rightEdge)
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
        if ((player.transform.position.x < transform.position.x && facingDirection > 0) ||
            (player.transform.position.x > transform.position.x && facingDirection < 0))
        {
            Flip();
        }

        if (!(player.transform.position.x > leftEdge && player.transform.position.x < rightEdge))
        {
            SwitchState(State.Walking);
        }
        
        
    }
    
    private void ExitRunningState()
    {
        animator.SetBool("Running", false);
    }

    
    // Standing State

    private void EnterStandingState()
    {
        
    }
    
    private void UpdateStandingState()
    {
        
    }
    
    private void ExitStandingState()
    {
        
    }
    
    // Hurt State

    private void EnterHurtState()
    {
        
    }
    
    private void UpdateHurtState()
    {
        
    }
    
    private void ExitHurtState()
    {
        
    }
    
    // Dead State

    private void EnterDeadState()
    {
        
    }
    
    private void UpdateDeadState()
    {
        
    }
    
    private void ExitDeadState()
    {
        
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
            case State.Hurt:
                ExitHurtState();
                break;
            case State.Dead:
                ExitDeadState();
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
            case State.Hurt:
                EnterHurtState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
        }

        currentState = state;
    }
    
    private void Flip()
    {
        facingDirection *= -1 ;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
    

}
