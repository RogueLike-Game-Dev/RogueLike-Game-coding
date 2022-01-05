using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVlad : MonoBehaviour
{

    [SerializeField] private GameObject nextLevelDoor;
    [SerializeField] private GameObject cascadeEnemies;
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            nextLevelDoor.GetComponent<LoadNextRoom>().enabled = true;
            cascadeEnemies.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
