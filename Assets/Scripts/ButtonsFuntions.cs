using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFuntions : MonoBehaviour
{
    private GameObject smallBoard;
    
    void Start()
    {
        smallBoard = GameObject.Find("SmallBoard");
        smallBoard.SetActive(false);
    }

    public void startGame()
    {
        RunStats.enemiesKilled = 0;
        RunStats.goldCollected = 0;
        RunStats.keysCollected = 0;
        RunStats.roomsCleared = 0;
        RunStats.playedTime = 0;
        RunStats.remainingHP = 10000;
        LoadNextRoom.LoadRoom();
    }

    public void CancelBanner()
    {
        smallBoard.SetActive(true);
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
}
