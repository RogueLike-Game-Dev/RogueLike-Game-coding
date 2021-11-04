using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWraith2 : MonoBehaviour
{
    private Animator wraithAnimator;
    public bool isShown = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject wraith2 = GameObject.Find("Wraith2");
            if (wraith2 != null)
            {
                wraith2.GetComponent<SpriteRenderer>().sortingOrder = 6;
                wraithAnimator = wraith2.GetComponent<Animator>();
                wraithAnimator.SetTrigger("isSeeing");
                isShown = true;
            }

        }
    }
}
