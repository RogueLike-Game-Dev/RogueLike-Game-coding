using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialOfTheMountainController : MonoBehaviour
{
    //parent objects
    public GameObject breakables;
    public GameObject gollemTurrets;
    public GameObject barricade;
    public GameObject fallTrap;

    private BoxCollider2D fallTrapCollider;
    //children
    private List<GameObject> breakablesActual;
    private List<GollemTurretController> gollemTurretsActual;

    public void Start()
    {
        fallTrapCollider = fallTrap.GetComponent<BoxCollider2D>();
        fallTrapCollider.isTrigger = true; //so that breakables can pass through
        breakablesActual = new List<GameObject>();
        gollemTurretsActual = new List<GollemTurretController>();


        Transform[] breakablesTransforms = breakables.GetComponentsInChildren<Transform>();
        foreach (Transform child in breakablesTransforms)
        {
            breakablesActual.Add(child.gameObject);
        }

        Transform[] gollemTurretsTransforms = gollemTurrets.GetComponentsInChildren<Transform>();
        foreach (Transform child in gollemTurretsTransforms)
        {
            gollemTurretsActual.Add(child.gameObject.GetComponent<GollemTurretController>());
            // Debug.Log(child.gameObject.name);
        }
    }

    // Start is called before the first frame update
    public void StartTrial()
    {
        StartCoroutine(Trial());
    }

    private IEnumerator Trial()
    {
        //set the main camera to be able to view all turrets and temporarily remove minimap
        GameObject player = GameObject.Find("Player");
        GameObject mainCamera = player.transform.GetChild(0).gameObject;
        mainCamera.GetComponent<Camera>().orthographicSize = 13;
        mainCamera.transform.SetParent(null, true);
        mainCamera.GetComponent<CameraFollow>().enabled = false;

        //add RigidBody2D to breakables;
        if (breakables != null) 
        {
            foreach (GameObject breakable in breakablesActual) 
            {
                Rigidbody2D rigidBody2D = breakable.AddComponent<Rigidbody2D>();
                rigidBody2D.AddForce(new Vector2(0f, -12f), ForceMode2D.Impulse);
            }
        }
        

        //wait for breakables to no longer be visible
        yield return new WaitForSeconds(1.5f);
        Destroy(breakables);
        fallTrapCollider.isTrigger = false;
        //start first firing pattern
        yield return SimpleFirePattern(gollemTurretsActual);
        //start second firing pattern
        yield return ComplexFirePattern(gollemTurretsActual);
        //destroy the barricade so the player can exit level
        Destroy(barricade);

        mainCamera.GetComponent<Camera>().orthographicSize = 10;
        mainCamera.transform.SetParent(player.transform, true);
        CameraFollow cameraScript = mainCamera.GetComponent<CameraFollow>();
        cameraScript.enabled = true;
        cameraScript.boxBounds = GameObject.Find("BoxBoundsReplacement").GetComponent<BoxCollider2D>();

    }

    //TODO: figure out camera settings in level script
    //TODO: ask about what a player prefab really needs
    //TODO: do Keybindings (look at the path provided by Paula) (maybe look up a tutorial if unsure where to begin)
    private IEnumerator SimpleFirePattern(List<GollemTurretController> gollemTurrets)
    {
        gollemTurrets[2].isActive = true;
        yield return new WaitForSeconds(0.75f);
        gollemTurrets[3].isActive = true;
        yield return new WaitForSeconds(0.75f);
        gollemTurrets[4].isActive = true;
        yield return new WaitForSeconds(0.75f);
        gollemTurrets[5].isActive = true;
        yield return new WaitForSeconds(2.5f);
        gollemTurrets[2].isActive = false;
        gollemTurrets[3].isActive = false;
        gollemTurrets[4].isActive = false;
        gollemTurrets[5].isActive = false;

        gollemTurrets[1].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[1].isActive = false;
        yield return new WaitForSeconds(1.5f);

        gollemTurrets[6].isActive = true;
        yield return new WaitForSeconds(0.75f);
        gollemTurrets[5].isActive = true;
        yield return new WaitForSeconds(0.75f);
        gollemTurrets[4].isActive = true;
        yield return new WaitForSeconds(0.75f);
        gollemTurrets[3].isActive = true;
        yield return new WaitForSeconds(2.5f);
        gollemTurrets[6].isActive = false;
        gollemTurrets[5].isActive = false;
        gollemTurrets[4].isActive = false;
        gollemTurrets[3].isActive = false;
        gollemTurrets[1].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[1].isActive = false;
        yield return new WaitForSeconds(1.5f);
        
    }

    private IEnumerator ComplexFirePattern(List<GollemTurretController> gollemTurrets)
    {
        yield return FireTwoTurretsOnce(gollemTurrets, 2, 6);

        yield return FireTwoTurretsOnce(gollemTurrets, 3, 5);

        gollemTurrets[4].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[4].isActive = false;
        yield return new WaitForSeconds(1f);

        yield return FireTwoTurretsOnce(gollemTurrets, 3, 5);

        yield return FireTwoTurretsOnce(gollemTurrets, 2, 6);

        for (int i = 2; i <= 6; i++)
        {
            gollemTurrets[i].fan = true;
            gollemTurrets[i].projectileNumber = 3;
        }

        gollemTurrets[1].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[1].isActive = false;

        yield return FireTwoTurretsOnce(gollemTurrets, 2, 6, 0.1f);

        gollemTurrets[1].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[1].isActive = false;

        yield return FireTwoTurretsOnce(gollemTurrets, 3, 5, 0.1f);

        gollemTurrets[1].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[1].isActive = false;

        gollemTurrets[4].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[4].isActive = false;
        yield return new WaitForSeconds(0.75f);

        gollemTurrets[1].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[1].isActive = false;

        yield return FireTwoTurretsOnce(gollemTurrets, 3, 5, 0.1f);

        gollemTurrets[1].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[1].isActive = false;

        yield return FireTwoTurretsOnce(gollemTurrets, 2, 6, 0.1f);

        yield return new WaitForSeconds(2f);
    }

    private IEnumerator FireTwoTurretsOnce(List<GollemTurretController> gollemTurrets, int a, int b, float delay=0f)
    {
        gollemTurrets[a].isActive = true;
        yield return new WaitForSeconds(delay);
        gollemTurrets[b].isActive = true;
        yield return new WaitForSeconds(0.2f);
        gollemTurrets[a].isActive = false;
        gollemTurrets[b].isActive = false;
        yield return new WaitForSeconds(0.75f);
    }


}
