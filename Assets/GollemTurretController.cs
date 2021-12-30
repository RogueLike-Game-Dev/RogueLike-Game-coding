//TODO:
//1. add fire up down at angles functionality 
//2. make projectile prefab
//3. implement animations
using System;
using System.Collections;
using UnityEngine;

public class GollemTurretController : MonoBehaviour
{
    public Rigidbody2D projectilePrefab;
    //Firing variables
    public float projectileSpeed = 10f;
    public int projectileNumber = 1;
    public bool fan = false; //shoots projectile in a fan instead of in a line
    public float timeBetweenAttacks = 1f;

    private bool isFiring;
    private GameObject target;
    [SerializeField]
    private bool facingRight;
    private Animator animator;
    private EntityStats gollemTurretStats;

    void Start()
    {
        target = GameObject.Find("Player");
        animator = GetComponent<Animator>();
        gollemTurretStats = GetComponent<EntityStats>(); 
        facingRight = transform.localScale.x > 0;
        isFiring = false;
    }

    void Update()
    {

        if (!isFiring){
            StartCoroutine(Fire(facingRight, projectileNumber, fan, timeBetweenAttacks));
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

    private void myInstantiate(Rigidbody2D copy, Vector2 pos, Vector2 acc, bool reverseX)
    {
        var projectileInst = Instantiate(copy, pos, Quaternion.identity);
        if (reverseX) 
        {
            projectileInst.velocity = new Vector2(-acc.x, acc.y);
        }
        else 
        {
            projectileInst.velocity = new Vector2(acc.x, acc.y);
        }
    }

    private IEnumerator Fire(bool facingRight, int projectileNumber, bool fan, float timeBetweenAttacks)
    {
        isFiring = true;
        // animator.SetTrigger(IS_FIRING);
        if (!fan) 
        {
            for (int i = 0; i < projectileNumber; i++)
            {
                myInstantiate(projectilePrefab, transform.position, new Vector2(projectileSpeed, 0), !facingRight);
                yield return new WaitForSeconds(0.15f);
            }
        }
        else 
        {
            if (projectileNumber == 1) 
            {
                myInstantiate(projectilePrefab, transform.position, new Vector2(projectileSpeed, 0), !facingRight);
            }
            else if (projectileNumber == 2) 
            {
                myInstantiate(projectilePrefab, transform.position, new Vector2(projectileSpeed, projectileSpeed/2), !facingRight);
                myInstantiate(projectilePrefab, transform.position, new Vector2(projectileSpeed, -projectileSpeed/2), !facingRight);
            }
            else 
            {
                for (int i = 0; i < projectileNumber; i++)
                {
                    myInstantiate(projectilePrefab, transform.position, new Vector2(projectileSpeed, projectileSpeed-(i*projectileSpeed/projectileNumber*2)), !facingRight);
                }
            }
            
        }
        
        
        yield return new WaitForSeconds(timeBetweenAttacks);
        isFiring = false;

    }
}
