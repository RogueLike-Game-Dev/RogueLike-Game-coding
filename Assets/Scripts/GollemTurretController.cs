//TODO:
//3. implement Dying and animations
using System;
using System.Collections;
using UnityEngine;

public class GollemTurretController : MonoBehaviour
{
    public Rigidbody2D projectilePrefab;
    //Firing variables
    public float projectileSpeed = 10f;
    public int projectileNumber = 1;
    public bool fan; //shoots projectile in a fan instead of in a line
    public float timeBetweenAttacks = 1f;
    public bool isActive;
    public bool invertXY;

    private bool isFiring;
    [SerializeField]
    private bool facingRightOrUp; 

    void Start()
    {
        isFiring = false;
    }

    void Update()
    {
        if (!isFiring && isActive){
            StartCoroutine(Fire(facingRightOrUp, projectileNumber, fan, timeBetweenAttacks));
        }
    }
    
    private void Flip()
    {
        var transformComponent = transform;
        facingRightOrUp = !facingRightOrUp;
        Vector3 theScale = transformComponent.localScale;
        theScale.x *= -1;
        transformComponent.localScale = theScale;
    }

    private void myInstantiate(Rigidbody2D copy, Vector2 pos, Vector2 acc, bool negateX=false, bool invertXY=false)
    {
        var projectileInst = Instantiate(copy, pos, Quaternion.identity);
        //for firing projectiles in the opposite direction
        if (negateX) 
        {
            acc.x = -acc.x;
        }
        //for firing projectile upwards/downwards
        if (invertXY) 
        {
            float temp = acc.x;
            acc.x = acc.y;
            acc.y = temp;
        }
        projectileInst.velocity = new Vector2(acc.x, acc.y);
    }

    private IEnumerator Fire(bool facingRightOrUp, int projectileNumber, bool fan, float timeBetweenAttacks)
    {
        isFiring = true;
        if (!fan) 
        {
            for (int i = 0; i < projectileNumber; i++)
            {
                myInstantiate(projectilePrefab, 
                              transform.position, 
                              new Vector2(projectileSpeed, 0), 
                              !facingRightOrUp, 
                              invertXY);

                yield return new WaitForSeconds(0.15f);
            }
        }
        else 
        {
            if (projectileNumber == 1) 
            {
                myInstantiate(projectilePrefab, 
                              transform.position, 
                              new Vector2(projectileSpeed, 0), 
                              !facingRightOrUp, 
                              invertXY);
            }
            else if (projectileNumber == 2) 
            {
                myInstantiate(projectilePrefab, 
                              transform.position, 
                              new Vector2(projectileSpeed, projectileSpeed/2), 
                              !facingRightOrUp, 
                              invertXY);

                myInstantiate(projectilePrefab, 
                              new Vector3(transform.position.x, transform.position.y-1, transform.position.z), 
                              new Vector2(projectileSpeed, -projectileSpeed/2), 
                              !facingRightOrUp, 
                              invertXY);
            }
            else 
            {
                for (int i = 0; i < projectileNumber; i++)
                {
                    myInstantiate(projectilePrefab, 
                                  new Vector3(transform.position.x-i, transform.position.y, transform.position.z), 
                                  new Vector2(projectileSpeed, projectileSpeed-(i*projectileSpeed/projectileNumber*2)), 
                                  !facingRightOrUp, 
                                  invertXY);
                }
            }
            
        }
        
        
        yield return new WaitForSeconds(timeBetweenAttacks);
        isFiring = false;

    }

}
