using System;
using UnityEngine;

public static class RunStats
{
    //selected save slot
    public static string selectedSlot = null;
    
    // played time
    public static string playedTime = "";
    public static string startTime = "";
    
    //settings
        //volume
        public static float volume = 0f;
        //keybindings single keys
        public static string jumpKey = KeyCode.W.ToString();
        public static string specialAttackKey = KeyCode.Mouse1.ToString();
        public static string basicAttackKey = KeyCode.Mouse0.ToString();
        public static string collectKey = KeyCode.G.ToString();
        public static string immunityKey = KeyCode.Alpha1.ToString();
        public static string invisibilityKey = KeyCode.Alpha1.ToString();
        //keybindings pair keys
        public static string dashKey = KeyCode.E + " " + KeyCode.S;
        
   //what user did in the run
    public static RoomType currentRoom;
    public static int goldCollected = 0;
    public static int keysCollected;
    public static int enemiesKilled = 0;
    public static int remainingHP = 10000;
    
    //what user unlocks
    public static string characters = "Zhax";  //add characters with space after, tried to avoid arrays
    //numer of unlocked features
    public static int armorMaxLevel;              
    public static int hpMaxLevel;                  
    public static int damageMaxLevel;              
    public static int speedMaxLevel;            
    public static int reviveNr;
    public static int immunityNr;             
    public static int invisibilityNr;  
    
    
}
