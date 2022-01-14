using System;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacterController : MonoBehaviour
{
    private Vector3 initialPlayerPosition = new Vector3(-100.0f, -100.0f, -100.0f);
    private Vector3 currentPlayerPosition;
    private float maxY;
    private int speed = 3;
    private float increaseValue = 0.05f;
    private GameObject player;
    private bool isBackToInitialPosition;
    private bool mouseOnCharacter;
    private Animator animator;

    public static int index = -1;

    void Update()
    {
        if (player != null && !isBackToInitialPosition && !mouseOnCharacter)
        {
            currentPlayerPosition = player.transform.localPosition;

            var targetPosition = initialPlayerPosition;
            var playerPosition = Vector3.MoveTowards(currentPlayerPosition, targetPosition, speed * Time.deltaTime);

            player.transform.localPosition = playerPosition;

            if (playerPosition == initialPlayerPosition)
            {
                isBackToInitialPosition = true;
            }
        }
    }

    private void OnMouseOver()
    {
        player = transform.GetChild(0).gameObject;
        currentPlayerPosition = player.transform.localPosition;
        animator = player.GetComponent<Animator>();
        
        if (Mathf.Approximately(currentPlayerPosition.y, maxY))
        {
            animator.SetBool("reachedMaxPos", true);
        }
        else
        {
            animator.SetBool("reachedMaxPos", false);
        }
        animator.SetBool("isJumping", true);
        
        isBackToInitialPosition = false;
        mouseOnCharacter = true;
        
        transform.GetChild(2).gameObject.SetActive(true);
        
        // set the initial position of the player
        if (Mathf.Approximately(initialPlayerPosition.x, -100.0f))
        {
            initialPlayerPosition = currentPlayerPosition;
            maxY = initialPlayerPosition.y + 0.6f;
        }
        
        var targetPosition = currentPlayerPosition;
        targetPosition.y += increaseValue;
        targetPosition.y = Math.Min(targetPosition.y, maxY);
        var playerPosition = Vector3.MoveTowards(currentPlayerPosition, targetPosition, speed * Time.deltaTime);

        player.transform.localPosition = playerPosition;
        
        var podiumName = transform.GetChild(1).gameObject.name;
        index = Int32.Parse(podiumName.Substring(podiumName.Length - 1)) - 1;
    }

    private void OnMouseExit()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("reachedMaxPos", false);
        
        mouseOnCharacter = false;
        transform.GetChild(2).gameObject.SetActive(false);

        index = -1;
    }
}
