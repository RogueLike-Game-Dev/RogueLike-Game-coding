using System;
using System.Collections;
using UnityEngine;

public class MinotaurController : MonoBehaviour
{

    private float speed;
    private GameObject target;
    private bool facingRight;
    private float distance;
    private Animator animator;
    private bool isWalking;
    private string IS_WALKING;
    private string IS_ATTACKING;
    private EntityStats minotaurStats;
    private GameObject attackArea;
    private DateTime lastAttackTime;

    void Start()
    {
        IS_WALKING = "isWalking";
        IS_ATTACKING = "isAttacking";
        target = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        minotaurStats = GetComponent<EntityStats>(); 
        facingRight = transform.localScale.x > 0;
        attackArea = transform.GetChild(0).gameObject;
        attackArea.SetActive(false);
        speed = minotaurStats.movementSpeed;
        isWalking = false;
        lastAttackTime = DateTime.Now;
    }

    void Update()
    {
        distance = Mathf.Abs(target.transform.position.x - transform.position.x);

        if (distance < 1.7f && target)  // enemy starts attacking the player
        {
            isWalking = false;
            animator.SetBool(IS_WALKING, isWalking);

            if (DateTime.Now.Subtract(lastAttackTime).TotalSeconds > 1.0)
            {
                lastAttackTime = DateTime.Now;
                StartCoroutine(Attack());
            }

            isWalking = true;
            animator.SetBool(IS_WALKING, isWalking);
        }
        else if (distance < 10.0f && target)     // enemy starts walking towards the player
        {
            // moving the enemy towards the player
            isWalking = true;
            animator.SetBool(IS_WALKING, isWalking);
            
            // flipping the enemy if the player gets close to it
            if (transform.position.x < target.transform.position.x && !facingRight)
            {
                Flip();
            }

            if (transform.position.x > target.transform.position.x && facingRight)
            {
                Flip();
            }
            
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        else
        {
            isWalking = false;
            animator.SetBool(IS_WALKING, isWalking);
        }
    }
    
    private void Flip()
    {
        var transformComponent = transform;
        facingRight = !facingRight;
        Vector3 theScale = transformComponent.localScale;
        theScale.x *= -1;
        transformComponent.localScale = theScale;
    }

    private IEnumerator Attack()
    {
        animator.SetTrigger(IS_ATTACKING);
        yield return new WaitForSeconds(0.8f);

        attackArea.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        attackArea.SetActive(false);
    }
}
