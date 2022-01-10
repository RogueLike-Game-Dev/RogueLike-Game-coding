using UnityEditor;
using UnityEngine;

public class ButtonsFuntions : MonoBehaviour
{
    private GameObject smallBoard;
    private GameObject settingsBoard;
    private GameObject volumeBoard;
    private GameObject keybindingsBoard;
    private GameObject creditsBoard;
    private GameObject saveSlot;

    void Start()
    {
        smallBoard = GameObject.Find("SmallBoard");
        settingsBoard = GameObject.Find("SettingsBoard");
        volumeBoard = GameObject.Find("VolumeBoard");
        keybindingsBoard = GameObject.Find("KeyBindings");
        creditsBoard = GameObject.Find("CreditsBoard");
        saveSlot = GameObject.Find("SaveSlotBoard");
        
        if (smallBoard)
        {
            smallBoard.SetActive(false);
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

    public void startGame()
    {
        RunStats.enemiesKilled = 0;
        RunStats.goldCollected = 0;
        RunStats.keysCollected = 0;
        RunStats.roomsCleared = 0;
        RunStats.playedTime = 0;
        RunStats.remainingHP = InitialValues.remainingHP;
        GameManager.isDying = false;
        GameManager.wasRevived = false;
        GameManager.pressed = false;
        LoadNextRoom.LoadRoom();
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
