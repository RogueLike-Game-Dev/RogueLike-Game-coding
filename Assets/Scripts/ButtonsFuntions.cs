using UnityEditor;
using UnityEngine;

public class ButtonsFuntions : MonoBehaviour
{
    private GameObject smallBoard;
    
    void Start()
    {
        smallBoard = GameObject.Find("SmallBoard");
        if (smallBoard)
        {
            smallBoard.SetActive(false);
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
