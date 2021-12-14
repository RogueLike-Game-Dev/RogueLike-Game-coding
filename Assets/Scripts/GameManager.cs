using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum RoomType { maxHP, gold, heal, powerUp};
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static GameObject player;
    private static EntityStats playerStats;

    private void Start()
    {
        player = GameObject.Find("Player");
        if(player != null)
            playerStats = player.GetComponent<EntityStats>();
    }
    
    private void Update()
    {
        RunStats.playedTime += Time.deltaTime;
    }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public static void EndRun()
    {
        Debug.Log("Player died, move to end screen");
        RunStats.enemiesKilled = playerStats.enemiesKilled;
        RunStats.goldCollected = playerStats.gold;
        
        SceneManager.LoadScene("EndGameScene", LoadSceneMode.Single);
    }

    public void SpawnCoins(int amount, Vector3 location) //TO DO: Spawn them in a cooler fashion
    {
        for (int i = 0; i < amount; i++)
        {
            var pooledCoin = ObjectPooler.Instance.GetPooledObject("Coin");
            pooledCoin.transform.position = location;
            pooledCoin.SetActive(true);
            pooledCoin.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.value, Random.value)*10;
        }
    }
    public void GenerateRoomsAndReward()
    {
        if (RunStats.currentRoom == RoomType.maxHP)
            Debug.Log("Spawning increased maxHP reward");
        else if (RunStats.currentRoom == RoomType.gold)
            Debug.Log("Spawning gold pack reward");
        else if (RunStats.currentRoom == RoomType.heal)
            Debug.Log("Spawning heal reward");
        else if (RunStats.currentRoom == RoomType.powerUp)
                  Debug.Log("Spawning powerUp reward");
    }
}
