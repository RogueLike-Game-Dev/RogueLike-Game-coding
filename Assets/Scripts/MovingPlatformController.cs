using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    private List<Vector3> targetPoints = new List<Vector3>();
    [SerializeField] private GameObject platform;
    [SerializeField] private float moveSpeed;

    private int currentTargetIndex = 0;
    // Update is called once per frame
    private void Start()
    {
        var transforms = gameObject.transform;
        foreach(Transform iter in transforms)
            targetPoints.Add(iter.position); //Add the starting point so it can cycle
    }

    void FixedUpdate()
    {
        if (platform.name != "Wraith2" || (platform.name == "Wraith2" && GameObject.Find("ShowWraith").GetComponent<ShowWraith2>().isShown))
        {
            if (platform.transform.position == targetPoints[currentTargetIndex]) // Platform got to current target
                currentTargetIndex = (currentTargetIndex + 1) % targetPoints.Count; //Move to next target

            platform.transform.position =
                Vector2.MoveTowards(platform.transform.position, targetPoints[currentTargetIndex], moveSpeed);
        }
    }
}
