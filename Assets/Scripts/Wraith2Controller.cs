using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wraith2Controller : MonoBehaviour
{
    private float distance;
    private Animator animator;
    private bool isAttacking = false;
    [SerializeField] private GameObject sword;
    private GameObject wraith2;
    private float throwingTime = 3f;
    private float timer = 2f;
    private float delay = 0.3f;
    SpriteRenderer spriteRenderer;
    [HideInInspector] public bool facingRight = true;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        wraith2 = GameObject.Find("Wraith2");
        spriteRenderer = wraith2.GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        if (wraith2 != null)
        {
            animator = wraith2.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        distance = Mathf.Abs(GameObject.Find("Player").transform.position.x - wraith2.transform.position.x);
        //Debug.Log(distance);
        if (distance < 7f)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", isAttacking);
            Debug.Log("Wraith2 attacks");
            //throw sward form its atack area to player
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartCoroutine(ThrowSwords());
                timer = 3f;
            }
        }
        else if (distance > 7f && isAttacking)
        {
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
        }

        if (player.transform.position.x < transform.position.x) //player is to wraith's left
        {
            if (facingRight)
            {
                Flip();
            }
        }
        else if (player.transform.position.x > transform.position.x) //player is to wraith's right
        {
            if (!facingRight)
            {
                Flip();
            }
        }
    }

    private IEnumerator ThrowSwords()
    {
        for (int i = 0; i <= 3; i++)
        {
            GameObject tempObj = Instantiate(sword, wraith2.transform.position, wraith2.transform.rotation);
            tempObj.GetComponent<SwordController>().target = player.transform.position;
            yield return new WaitForSeconds(delay);
        }
    }
    
    private void Flip()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX =! spriteRenderer.flipX;
    }
}