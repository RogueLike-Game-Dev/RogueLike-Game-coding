using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningRocks : MonoBehaviour
{
    //wave spawner for rocks
    [SerializeField] private GameObject gameObject; //rocks to spawn
    [SerializeField] private float spawnTime = 1f;
    private float spawnDelay = 5f;
    [SerializeField] private bool stopSpawning = false;
    private Rigidbody2D rigidbody;
    
    private void ThrowRocks()
    {
        GameObject tempObj = Instantiate(gameObject, transform.position, transform.rotation);
        rigidbody = tempObj.gameObject.GetComponent<Rigidbody2D>();
        rigidbody.AddForce(Vector2.left, ForceMode2D.Impulse); //new Vector2(-1f, 0f), ForceMode2D.Impulse);
        if (stopSpawning)
        {
            CancelInvokeRocks();
        }
    }

    public void InvokeRocks()
    {
        InvokeRepeating("ThrowRocks", spawnTime, spawnDelay);
    }
    
    public void CancelInvokeRocks()
    {
        CancelInvoke("ThrowRocks");
    }
}
