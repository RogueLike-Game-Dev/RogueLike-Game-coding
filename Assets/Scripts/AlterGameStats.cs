using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlterGameStats : MonoBehaviour
{
    [SerializeField] private Text enemiesKilled;
    [SerializeField] private Text gold;
    [SerializeField] private Text time;
    
    // Start is called before the first frame update
    
    void Start()
    {
        enemiesKilled.text = RunStats.enemiesKilled == 0 ? "0 wraiths" : RunStats.enemiesKilled.ToString() + " wraiths";
        gold.text = RunStats.goldCollected == 0 ? "0 pesos" : RunStats.goldCollected.ToString() + " pesos";
        string minutes = Mathf.Floor(RunStats.playedTime / 60).ToString("00");
        string seconds = (RunStats.playedTime % 60).ToString("00");
        time.text = string.Format("{0} : {1}", minutes, seconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
