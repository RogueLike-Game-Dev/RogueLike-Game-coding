using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WraithController : MonoBehaviour
{
   
    private Transform target;
    private float speed;
    private bool facingRight = true;
   
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        speed = this.GetComponent<EntityStats>().movementSpeed;
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null ) //If player is not dead
        {
           
            if (transform.position.x < target.position.x && !facingRight) //Wraith is left to player (so he has to move to right) and is not facingRight => need to flip
                Flip();
            if (transform.position.x > target.position.x && facingRight) //Wraith is right to player (so he has to move to left) and is facingRight => need to flip
                Flip();
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
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
