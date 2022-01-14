using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LoadNextRoom : MonoBehaviour
{
    private bool triggered;
    private static int currentSceneIndex;
    private static int roomsCreated = 0;
    private const int minIndex = 1;     // TODO: also include the Boss Scene (minIndex = 0) 
    private const int maxIndex = 5; 
    private EntityStats playerStats;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerStats = GameObject.Find("Player").GetComponent<EntityStats>();
   
    }

    private void Update()
    {
        if (triggered)
        {
            Debug.Log("Player is at next level door");
            if (Input.GetKey(KeyCode.G))
            {
                Debug.Log("Loading next room");
                playerStats.gold = RunStats.goldCollected;
                LoadRoom();
              
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered next level door trigger");
            triggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            triggered = false;
        }
    }

    public static void LoadRoom()
    {
      
        roomsCreated += 1;
        if (Random.Range(0, 1) > (1f / roomsCreated))
        { SceneManager.LoadScene("BossScene");
        return; }
        var sceneIndex = Random.Range(minIndex, maxIndex);
        while (sceneIndex == currentSceneIndex)
        {
            sceneIndex = Random.Range(minIndex, maxIndex);
        }

        print(sceneIndex);
        currentSceneIndex = sceneIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}
