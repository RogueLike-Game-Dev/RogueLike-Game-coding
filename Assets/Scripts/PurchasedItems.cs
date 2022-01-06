using System;

[Serializable]
public class PurchasedItems
{
    public int armorMaxLevel;               // ok
    public int hpMaxLevel;                  // ok
    public int damageMaxLevel;              // magical damage must be added where the secondary attack is (eg: for Zhax -> ThrowingObjectController.cs)
    public int speedMaxLevel;               // ok
    public int reviveNr;
    public int immunityNr;                  // ok
    public int invisibilityNr;              // condition of invisibility must be used in the enemy's controller 
    private static PurchasedItems instance;

    public static PurchasedItems getInstance()
    {
        if (instance == null)
        {
            instance = new PurchasedItems();
        }

        return instance;
    }
}
