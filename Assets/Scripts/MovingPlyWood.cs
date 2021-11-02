using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlyWood : MonoBehaviour
{
    private float limitRight;
    [SerializeField] private float speed = 0.1f;
    private List<Vector3> targetPoints = new List<Vector3>();

    private int currentTargetIndex = 0;
    
    private void Start()
    {
        limitRight = GameObject.Find("LimitRight").transform.position.x;
        targetPoints.Add(gameObject.transform.position);
        targetPoints.Add( new Vector3(limitRight, gameObject.transform.position.y, gameObject.transform.position.z));
        
    }
    
    void FixedUpdate()
    {
        if (gameObject.transform.position == targetPoints[currentTargetIndex]) // Platform got to current target
            currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Count; //Move to next target
        
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPoints[currentTargetIndex], speed);
    }
}