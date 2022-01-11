using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LoadNextRoom : MonoBehaviour
{
    private bool triggered;
    private static int currentSceneIndex;
    private const int minIndex = 1;     // TODO: also include the Boss Scene (minIndex = 0) 
    private const int maxIndex = 6; 
    private EntityStats playerStats;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        playerStats = GameObject.Find("Player").GetComponent<EntityStats>();
        if (playerStats != null)
        {
            RunStats.remainingHP = playerStats.maxHP;
        }
    }

    private void Update()
    {
        if (triggered)
        {
            if (Input.GetKey(KeyCode.G))
            {
                LoadRoom();
                playerStats.gold = RunStats.goldCollected;
                RunStats.remainingHP = playerStats.currentHP;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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
