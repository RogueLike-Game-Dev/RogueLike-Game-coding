using UnityEngine;

public static class RunStats
{
    public static int roomsCleared = 0;
    public static int enemiesKilled = 0;
    public static RoomType currentRoom;
    public static int goldCollected = 0;
    public static float playedTime = 0f;
    public static Time startTime;
    public static int keysCollected;
    public static int remainingHP = InitialValues.remainingHP;
}
