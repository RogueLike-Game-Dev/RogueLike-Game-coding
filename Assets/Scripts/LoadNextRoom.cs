using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LoadNextRoom : MonoBehaviour
{
    private bool triggered;
    private int currentSceneIndex;
    private const int minIndex = 1;
    private const int maxIndex = 6;

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        if (triggered)
        {
            LoadRoom();            
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

    private void LoadRoom()
    {
        if (Input.GetKey(KeyCode.G))
        {
            var sceneIndex = Random.Range(minIndex, maxIndex);
            while (sceneIndex == currentSceneIndex)
            {
                sceneIndex = Random.Range(minIndex, maxIndex);
            }
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
