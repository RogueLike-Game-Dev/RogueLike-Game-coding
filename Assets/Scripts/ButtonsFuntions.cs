using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsFuntions : MonoBehaviour
{
    private GameObject smallBoard;
    // Start is called before the first frame update
    void Start()
    {
        smallBoard = GameObject.Find("SmallBoard");
        smallBoard.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame() //@TODO when generating rooms is finished
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
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
