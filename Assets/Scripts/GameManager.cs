using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum RoomType { maxHP, gold, heal, powerUp};
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static GameObject player;
    private static GameObject closeBanner;
    private static EntityStats playerStats;
    [SerializeField] private GameObject esterosPrefab;
    [SerializeField] private GameObject demetriaPrefab;
    [SerializeField] private GameObject zhaxPrefab;
    [SerializeField] private GameObject lynPrefab;
    private static GameObject playerPrefab;
    private static int count = 5;
    public static bool pressed;
    public static bool isDying;

    [SerializeField] private GameObject parchment;
    [SerializeField] private GameObject counter;
    [SerializeField] private GameObject revive;
    [SerializeField] private GameObject reviveImage;

    public static bool wasRevived;
    private static Vector3 deathPosition;
    public static bool makeInvulnerableAfterRevive;
    
    private void Start()
    {
        player = GameObject.Find("Player");
        closeBanner = GameObject.Find("CanvasCloseBanner");
        if (player != null)
        {
            playerStats = player.GetComponent<EntityStats>();
            playerStats.gold = RunStats.goldCollected;
            Debug.Log("gold" + playerStats.gold);
            Debug.Log("gold" + RunStats.goldCollected);
        }
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject closeBoard = closeBanner.transform.GetChild(0).gameObject;
            if(closeBoard) closeBoard.SetActive(true);
            else Application.Quit();
        }

        if (isDying)
        {
            Activate();
            if (PurchasedItems.getInstance().reviveNr >= 1)
            {
                revive.GetComponent<Text>().text = "Press F to revive";
                reviveImage.SetActive(true);
            }
            else
            {
                revive.GetComponent<Text>().text = "No more revives left";
                reviveImage.SetActive(false);
            }
        }
        
        if (isDying && Input.GetKeyDown(KeyCode.F))
        {
            if (PurchasedItems.getInstance().reviveNr >= 1 && !wasRevived)
            {
                pressed = true;
                Deactivate();
                isDying = false;
                wasRevived = true;

                playerStats.currentHP = playerStats.maxHP / 2;

                PurchasedItems.getInstance().reviveNr--;

                // relocate the player:
                // - at the beginning of the room, if he died by falling
                // - at the death position, being invulnerable, if he died in fight
                if (player.transform.position.y <= -30) // dead by falling
                {
                    player.transform.position = player.GetComponent<PlayerMovement>().initialPosition;
                }
                else    // dead in fight
                {
                    player.transform.position = deathPosition;
                    makeInvulnerableAfterRevive = true;
                }
            }
            else
            {
                revive.GetComponent<Text>().text = "No more revives left";
                reviveImage.SetActive(false);
                print("CANNOT REVIVE TWICE OR DONT HAVE ENOUGH ITEMS " + PurchasedItems.getInstance().reviveNr);
            }
        }
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
        
        var type = PlayerMovement.characterType;

        if (type.Equals(PlayerMovement.CharacterType.Demetria))
        {
            playerPrefab = demetriaPrefab;
        }
        else if (type.Equals(PlayerMovement.CharacterType.Esteros))
        {
            playerPrefab = esterosPrefab;
        }
        else if (type.Equals(PlayerMovement.CharacterType.Zhax))
        {
            playerPrefab = zhaxPrefab;
        }
        else if (type.Equals(PlayerMovement.CharacterType.Lyn))
        {
            playerPrefab = lynPrefab;
        }
        

        if (SceneManager.GetActiveScene().name != "EndGameScene" && GameObject.FindWithTag("Player") == null)
        {
            var playerInstance = Instantiate(playerPrefab);
            playerInstance.transform.position = Vector3.zero;
            playerInstance.name = "Player";
        }
    }
    public static void EndRun()
    {
        if (wasRevived)
        {
            LoadRoomAfterDeath();
        }
        else
        {
            deathPosition = player.transform.position;
            
            if (isDying)
            {
                return;
            }
            
            count = 5;  
            playerStats.StartCoroutine(Count());
        }
    }

    private void Activate()
    {
        parchment.SetActive(true);
        counter.SetActive(true);
        revive.SetActive(true);
        reviveImage.SetActive(true);
    }
    
    private void Deactivate()
    {
        parchment.SetActive(false);
        counter.SetActive(false);
        revive.SetActive(false);
        reviveImage.SetActive(false);
    }

    private static void LoadRoomAfterDeath()
    {
        Debug.Log("Player died, move to end screen");
        RunStats.enemiesKilled = playerStats.enemiesKilled;
        RunStats.goldCollected = playerStats.gold;
        
        DateTime endGameTime = System.DateTime.Now;
        DateTime startGame =  DateTime.Parse(RunStats.startTime);
        TimeSpan span = endGameTime - startGame;
        if (RunStats.playedTime != "")
        {
            string[] subs  = RunStats.playedTime.Split(' ');
            List <int> time = new List<int>(); //h min sec
            Debug.Log("GAME MANAGER SAVING TIME 1");
            foreach (string str in subs)
            {
                Debug.Log(str);
                int nr = 0;
                if (int.TryParse(str, out nr))
                {
                    time.Add(nr);
                }
            }
            DateTime alreadyPlayedTime = new DateTime(endGameTime.Year, endGameTime.Month, endGameTime.Day, time[0], time[1], time[2]);
            DateTime newPlayedTime = alreadyPlayedTime.AddHours(span.Hours).AddMinutes(span.Minutes).AddSeconds(span.Seconds);

            RunStats.playedTime = String.Format("{0} h: {1} min: {2} sec", newPlayedTime.Hour, newPlayedTime.Minute, newPlayedTime.Second);
            Debug.Log("New played Time " + RunStats.playedTime);

        }
        else
        {
            RunStats.playedTime = String.Format("{0} h: {1} min: {2} sec", span.Hours, span.Minutes, span.Seconds);
            Debug.Log("Played Time " + RunStats.playedTime);
        }

        DateTime startPlayingTime = System.DateTime.Now;
        RunStats.startTime = startPlayingTime.ToString();
        SaveLoadSystem.SaveRun(); //save previous run
        SceneManager.LoadScene("EndGameScene", LoadSceneMode.Single);
    }

    private static IEnumerator Count()
    {
        isDying = true;
        while (count >= 0)
        {
            if (pressed)
            {
                break;
            }
            
            var countText = GameObject.Find("Counter");
            if (countText)
            {
                countText.GetComponent<Text>().text = count.ToString();
            }
            yield return new WaitForSeconds(1.0f);
            count--;
        }

        isDying = false;
        
        if (!pressed)
        {
            LoadRoomAfterDeath();
        }
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
