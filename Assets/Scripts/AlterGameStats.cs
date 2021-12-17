using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlterGameStats : MonoBehaviour
{
    [SerializeField] private Text enemiesKilled;
    [SerializeField] private Text gold;
    [SerializeField] private Text time;
    
    void Start()
    {
        var enemyText = " enemies";
        if (RunStats.enemiesKilled == 1)
        {
            enemyText = " enemy";
        }
        
        enemiesKilled.text = RunStats.enemiesKilled + enemyText;
        gold.text = RunStats.goldCollected + " pesos";
        
        var minutes = Mathf.Floor(RunStats.playedTime / 60).ToString("00");
        var seconds = (RunStats.playedTime % 60).ToString("00");
        time.text = string.Format("{0} : {1}", minutes, seconds);
    }
}
