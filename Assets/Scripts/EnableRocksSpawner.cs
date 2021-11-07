using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRocksSpawner : MonoBehaviour
{
    private List<GameObject> spawners;

    // Start is called before the first frame update
    void Start()
    {
        spawners = new List<GameObject>(4);
        spawners.Add(GameObject.Find("Rock1ToSpawn"));
        spawners.Add(GameObject.Find("Rock2ToSpawn"));
        spawners.Add(GameObject.Find("Rock3ToSpawn"));
        spawners.Add(GameObject.Find("Rock4ToSpawn"));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (GameObject.Find("Player").GetComponent<PlayerMovement>().facingRight)
            {
                foreach (var varSpawner in spawners)
                {
                    varSpawner.GetComponent<SpawningRocks>().InvokeRocks();
                }
            }
            else
            {
                foreach (var varSpawner in spawners)
                {
                    varSpawner.GetComponent<SpawningRocks>().CancelInvokeRocks();
                }
            }
        }
    }
}
