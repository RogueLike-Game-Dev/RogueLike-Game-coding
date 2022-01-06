using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithController : MonoBehaviour
{
   
    private Transform target;
    private EntityStats targetStats;
    private float speed;
    private bool facingRight = true;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private const float randomRange = 3.0f;
   
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        speed = this.GetComponent<EntityStats>().movementSpeed;
        initialPosition = transform.position;

        if (target)
        {
            targetStats = target.gameObject.GetComponent<EntityStats>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if ((target != null && targetStats.isInvisible) || GameManager.isDying)  // if player is invisible, set target position for wraith
        {
            if (transform.position == initialPosition)
            {
                var random1 = Random.Range(-randomRange, randomRange);
                var random2 = Random.Range(-randomRange, randomRange);
                var newPos = new Vector3(initialPosition.x + random1,
                    initialPosition.y + random2,
                    initialPosition.z);
                targetPosition = newPos;
            }
            else if (transform.position == targetPosition)
            {
                targetPosition = initialPosition;
            }
        }
        
        if (target != null) //If player is not dead
        {
            if (!targetStats.isInvisible && !GameManager.isDying)
            {
                if (transform.position.x < target.position.x &&
                    !facingRight) //Wraith is left to player (so he has to move to right) and is not facingRight => need to flip
                    Flip();
                if (transform.position.x > target.position.x &&
                    facingRight) //Wraith is right to player (so he has to move to left) and is facingRight => need to flip
                    Flip();
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else
            {
                if (transform.position.x < targetPosition.x && !facingRight)
                    Flip();
                if (transform.position.x > targetPosition.x && facingRight)
                    Flip();
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
        }
   
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("WraithTriggerEnter");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Wraith Collision");
    }
}
