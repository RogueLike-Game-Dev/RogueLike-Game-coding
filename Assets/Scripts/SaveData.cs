using System;
using UnityEngine;

[Serializable]
public class SaveData
{
   public string playedTime;
   public string startTime;
   public float volume;
   public string jumpKey;
   public string specialAttackKey;
   public string basicAttackKey;
   public string collectKey;
   public string immunityKey;
   public string invisibilityKey;
   public string dashKey;
   public RoomType currentRoom;
   public int goldCollected;
   public int keysCollected;
   public int enemiesKilled;
   public int remainingHP;
   public string characters;
   public int armorMaxLevel;
   public int hpMaxLevel;
   public int damageMaxLevel;
   public int speedMaxLevel;
   public int reviveNr;
   public int immunityNr;
   public int invisibilityNr;

   public SaveData()
   {
      playedTime = RunStats.playedTime;
      startTime = RunStats.startTime;
      volume = RunStats.volume;
      jumpKey = RunStats.jumpKey;
      specialAttackKey = RunStats.specialAttackKey;
      basicAttackKey = RunStats.basicAttackKey;
      collectKey = RunStats.collectKey;
      immunityKey = RunStats.immunityKey;
      invisibilityKey = RunStats.invisibilityKey;
      dashKey = RunStats.dashKey;
      currentRoom = RunStats.currentRoom;
      goldCollected = RunStats.goldCollected;
      keysCollected = RunStats.keysCollected;
      enemiesKilled = RunStats.enemiesKilled;
      characters = "Zhax"; 
      armorMaxLevel =  RunStats.armorMaxLevel;
      hpMaxLevel = RunStats.hpMaxLevel;
      damageMaxLevel = RunStats.damageMaxLevel;
      speedMaxLevel = RunStats.speedMaxLevel;
      reviveNr = RunStats.reviveNr;
      immunityNr = RunStats.immunityNr;
      invisibilityNr = RunStats.invisibilityNr;
      
   }

}