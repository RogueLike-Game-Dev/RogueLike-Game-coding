using System;

[Serializable]
public class InitialValues
{
    // ??
    // remainingHP must be initialized with player's maxHP
    // calculated using the information in the save file
    // (add the amount according to the hpMaxLevel value)
    public static int remainingHP = 10000;
}
