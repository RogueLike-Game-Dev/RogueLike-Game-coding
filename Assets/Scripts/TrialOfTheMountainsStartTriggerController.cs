using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialOfTheMountainsStartTriggerController : MonoBehaviour
{

    public GameObject TOMController;
    private TrialOfTheMountainController TOMScript;

    void Start()
    {
        TOMScript = TOMController.GetComponent<TrialOfTheMountainController>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            TOMScript.StartTrial();
        }
    }

}
