using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsFuntions : MonoBehaviour
{
    private GameObject smallBoard;
    private GameObject settingsBoard;
    private GameObject volumeBoard;
    private GameObject keybindingsBoard;
    private GameObject creditsBoard;
    private GameObject saveSlot;
    private GameObject continueGameButton;
    private List<GameObject> saveSlots;
    public Slider mSlider;

    void Start()
    {
        smallBoard = GameObject.Find("SmallBoard");
        settingsBoard = GameObject.Find("SettingsBoard");
        volumeBoard = GameObject.Find("VolumeBoard");
        keybindingsBoard = GameObject.Find("KeyBindings");
        creditsBoard = GameObject.Find("CreditsBoard");
        saveSlot = GameObject.Find("SaveSlotBoard");
        continueGameButton =  GameObject.Find("ContinueGame");
        saveSlots = new List<GameObject>();
        
        for (int i = 1; i <= 5; i++)
        {
            GameObject slot = GameObject.Find("SaveRun" + i);
            if (slot)
            {
                slot.SetActive(false);
                saveSlots.Add(slot);
            }
        }
        
        if (smallBoard)
        {
            smallBoard.SetActive(false);
        }
        
        if (continueGameButton)
        {
            continueGameButton.SetActive(false);
        }
        
        if (settingsBoard != null && volumeBoard != null && keybindingsBoard != null 
            && creditsBoard != null && saveSlot != null)
        {
            settingsBoard.SetActive(false);
            volumeBoard.SetActive(false);
            keybindingsBoard.SetActive(false);
            creditsBoard.SetActive(false);
            saveSlot.SetActive(false);
        }
    }

    public void startGame()  //start new game
    {
        DateTime startPlayingTime = System.DateTime.Now;
        RunStats.startTime = startPlayingTime.ToString();
        Debug.Log("Starting game at: " + RunStats.startTime);
        
        RunStats.enemiesKilled = 0;
        RunStats.goldCollected = 0;
        RunStats.keysCollected = 0;
        RunStats.remainingHP = InitialValues.remainingHP;
        GameManager.isDying = false;
        GameManager.wasRevived = false;
        GameManager.pressed = false;
        LoadNextRoom.LoadRoom();
    }
    
    public void startGameAfterDying()
    {
        SaveLoadSystem.SaveRun(); //save previous run
        SceneManager.LoadScene("StartScene");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("ShopScene");
        
    }

    public void CancelBanner()
    {
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

    public void selectSlot()
    {
        string saveLotName = EventSystem.current.currentSelectedGameObject.name;
        SaveData runData = SaveLoadSystem.LoadRun(saveLotName);
        if (runData != null)
        {
            Debug.Log("Continue Run");
            RunStats.selectedSlot = saveLotName;
            RunStats.enemiesKilled = runData.enemiesKilled;
            RunStats.goldCollected = runData.goldCollected;
            RunStats.keysCollected = runData.keysCollected;
            Music gameMusic = GameObject.Find("Background").GetComponent<Music>();
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
