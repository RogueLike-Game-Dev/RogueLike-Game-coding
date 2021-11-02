using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWraith2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Wraith2").GetComponent<SpriteRenderer>().sortingOrder = 6;
            
        }
    }
}
