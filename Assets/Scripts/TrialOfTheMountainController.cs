using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialOfTheMountainController : MonoBehaviour
{
    //parent objects
    public GameObject breakables;
    public GameObject gollemTurrets;
    public GameObject barricade;

    //children
    private List<GameObject> breakablesActual;
    private List<GameObject> gollemTurretsActual;

    public void Start()
    {
        breakablesActual = new List<GameObject>();
        gollemTurretsActual = new List<GameObject>();

        Transform[] breakablesTransforms = breakables.GetComponentsInChildren<Transform>();
        foreach (Transform child in breakablesTransforms)
        {
            breakablesActual.Add(child.gameObject);
        }

        Transform[] gollemTurretsTransforms = gollemTurrets.GetComponentsInChildren<Transform>();
        foreach (Transform child in gollemTurretsTransforms)
        {
            gollemTurretsActual.Add(child.gameObject);
        }
    }

    // Start is called before the first frame update
    public void StartTrial()
    {
        StartCoroutine(Trial());
    }

    private IEnumerator Trial()
    {
        //add RigidBody2D to breakables;
        foreach (GameObject breakable in breakablesActual) 
        {
            Rigidbody2D rigidBody2D = breakable.AddComponent<Rigidbody2D>();
            rigidBody2D.AddForce(new Vector2(0f, -8f), ForceMode2D.Impulse);
        }

        //wait for breakables to no longer be visible
        yield return new WaitForSeconds(2.5f);
        Destroy(breakables);
        //start first firing pattern
        yield return SimpleFirePattern(gollemTurrets);
        //start second firing pattern
        yield return ComplexFirePattern(gollemTurrets);
        //destroy the barricade so the player can exit level
        Destroy(barricade);

    }

    private IEnumerator SimpleFirePattern(GameObject gollemTurrets)
    {
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator ComplexFirePattern(GameObject gollemTurrets)
    {
        yield return new WaitForSeconds(2f);
    }

}
