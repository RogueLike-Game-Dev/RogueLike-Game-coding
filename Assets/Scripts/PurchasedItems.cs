using System;

[Serializable]
public class PurchasedItems
{
    public int armorMaxLevel;               // ok, TODO armor UI
    public int hpMaxLevel;                  // ok
    public int damageMaxLevel;              // magical damage must be added where the secondary attack is (eg: for Zhax -> ThrowingObjectController.cs)
    public int speedMaxLevel;               // ok
    public int reviveNr;
    public int immunityNr;
    public int invisibilityNr; 
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
