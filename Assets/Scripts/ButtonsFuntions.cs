using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsFuntions : MonoBehaviour
{
    [SerializeField] private GameObject smallBoard;
    [SerializeField] private GameObject settingsBoard;
    [SerializeField] private GameObject volumeBoard;
    [SerializeField] private GameObject keybindingsBoard;
    [SerializeField] private GameObject creditsBoard;
    [SerializeField] private GameObject saveSlot;
    [SerializeField] private GameObject continueGameButton;
    [SerializeField] private GameObject emptySlots;
    [SerializeField] private List<GameObject> saveSlots = new List<GameObject>();
    public Slider mSlider;

    void Start()
    {

       
    }

    public void startGame()  //start new game
    {
        DateTime startPlayingTime = System.DateTime.Now;
        RunStats.startTime = startPlayingTime.ToString();
        Debug.Log("Starting game at: " + RunStats.startTime);
        RunStats.remainingHP = InitialValues.remainingHP;
        GameManager.isDying = false;
        GameManager.wasRevived = false;
        GameManager.pressed = false;
        LoadNextRoom.LoadRoom();
    }
    
    public void startGameAfterDying()
    {
        SaveLoadSystem.SaveRun(); //save previous run
        LoadNextRoom.LoadRoom();
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("ShopScene");
        
    }

    public void CancelBanner()
    {
        if (RunStats.selectedSlot != null)
        {
            SaveLoadSystem.SaveRun();
        }
        smallBoard.SetActive(true);
    }

    public void OpenSettingsBoard()
    {
        settingsBoard.SetActive(true);
    }
    
    public void OpenSaveSlotBoard()
    {
        saveSlot.SetActive(true);
        List<SaveData> savedRuns = SaveLoadSystem.LoadRuns();
        Debug.Log("Runs found:" + savedRuns.Count);
        if (savedRuns.Count == 0)
        {
            emptySlots.SetActive(true);
            return; 
        }
        
        //order runs by date played
        savedRuns.Sort((a, b) => - a.startTime.CompareTo(b.startTime));
        int slotNo = 0;
        foreach (SaveData run in savedRuns)
        {
            
            string text = run.startTime + ", " + run.playedTime + " played, " + run.goldCollected + " pesos collected";
            GameObject slot = saveSlots[slotNo];
            slot.SetActive(true);
            slot.GetComponentInChildren<Text>().text = text;
            slotNo++;
        }
    }

    public void selectSlot(int i)
    {
        Debug.Log("Selected Slot");
        string saveSlotName = "SaveRun" + i;
        RunStats.selectedSlot = saveSlotName;
        SaveData runData = SaveLoadSystem.LoadRun(saveSlotName);
        if (runData != null)
        {
            Debug.Log("Continue Run");
            RunStats.selectedSlot = saveSlotName;
            RunStats.enemiesKilled = runData.enemiesKilled;
            RunStats.goldCollected = runData.goldCollected;
            RunStats.playedTime = runData.playedTime;
            Debug.Log("Previous Gold collected:" + RunStats.goldCollected);
            RunStats.keysCollected = runData.keysCollected;
            Music gameMusic = GameObject.Find("AudioSource").GetComponent<Music>();
            if (gameMusic)
            {
                gameMusic.ChangeVolume((float)runData.volume);
                mSlider.value = runData.volume;
                Debug.Log("Game music volume: " + runData.volume);
            }
            
            continueGameButton.SetActive(true);
            GameObject newGameButton = GameObject.Find("NewStart");
            if (newGameButton)
            {
                newGameButton.SetActive(false);
            }

            CloseSaveSlotBanner();
        }

    }

    public void OpenVolumeBoard()
    {
        volumeBoard.SetActive(true);
    }

    public void OpenCreditsBoard()
    {
        creditsBoard.SetActive(true);
    }
    
    public void OpenKeybindingsBoard()
    {
        keybindingsBoard.SetActive(true);
    }

    public void GameQuit()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void CloseSmallBanner()
    {
        smallBoard.SetActive(false);
    }
    
    public void CloseCreditsBanner()
    {
        creditsBoard.SetActive(false);
    }
    
    public void CloseSettingsBanner()
    {
        settingsBoard.SetActive(false);
    }
    
    public void CloseSaveSlotBanner()
    {
        saveSlot.SetActive(false);
    }
    
    public void CloseVolumeBoard()
    {
        volumeBoard.SetActive(false);
    }

    public void CloseKeybindingsBoard()
    {
        keybindingsBoard.SetActive(false);
    }
}
