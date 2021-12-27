using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillJSON
{
    public int maxLevel;
    public string title;
    public string[] descriptions;
    public int[] prices;
}

public class SpendGoldController : MonoBehaviour
{
    private const int nrButtons = 18;
    [SerializeField] public Button[] buttons = new Button[nrButtons];

    private int playerGold = 1000000000;     // must update this when player finished a run (by adding the gold collected in that run)
                                            // OR getting it from the save file (when player loads the game)

    private GameObject infoCanvas;

    #region skillobjects
    private SkillJSON armorSkill;
    private SkillJSON hpRegenSkill;
    private SkillJSON hpSkill;
    private SkillJSON damageSkill;
    private SkillJSON magicalDamageSkill;
    private SkillJSON movementSpeedSkill;
    private SkillJSON jumpSkill;
    #endregion

    void Start()
    {
        // initializing skill objects
        armorSkill = new SkillJSON();
        hpRegenSkill = new SkillJSON();
        hpSkill = new SkillJSON();
        damageSkill = new SkillJSON();
        magicalDamageSkill = new SkillJSON();
        movementSpeedSkill = new SkillJSON();
        jumpSkill = new SkillJSON();

        // armor skill info
        armorSkill.maxLevel = 3;
        armorSkill.title = "Armor";
        armorSkill.descriptions = new string[armorSkill.maxLevel];
        armorSkill.descriptions[0] = "armor 1";
        armorSkill.descriptions[1] = "armor 2";
        armorSkill.descriptions[2] = "armor 3";
        armorSkill.prices = new int[armorSkill.maxLevel];
        armorSkill.prices[0] = 1000;
        armorSkill.prices[1] = 5000;
        armorSkill.prices[2] = 15000;
        
        // hp regen skill info
        hpRegenSkill.maxLevel = 3;
        hpRegenSkill.title = "Health Regeneration";
        hpRegenSkill.descriptions = new string[hpRegenSkill.maxLevel];
        hpRegenSkill.descriptions[0] = "hpRegenSkill 1";
        hpRegenSkill.descriptions[1] = "hpRegenSkill 2";
        hpRegenSkill.descriptions[2] = "hpRegenSkill 3";
        hpRegenSkill.prices = new int[hpRegenSkill.maxLevel];
        hpRegenSkill.prices[0] = 2000;
        hpRegenSkill.prices[1] = 10000;
        hpRegenSkill.prices[2] = 20000;
        
        // hp skill info
        hpSkill.maxLevel = 3;
        hpSkill.title = "Health Booster";
        hpSkill.descriptions = new string[hpSkill.maxLevel];
        hpSkill.descriptions[0] = "hpSkill 1";
        hpSkill.descriptions[1] = "hpSkill 2";
        hpSkill.descriptions[2] = "hpSkill 3";
        hpSkill.prices = new int[hpSkill.maxLevel];
        hpSkill.prices[0] = 1500;
        hpSkill.prices[1] = 5000;
        hpSkill.prices[2] = 12000;
        
        // damage skill info
        damageSkill.maxLevel = 3;
        damageSkill.title = "Damage Booster";
        damageSkill.descriptions = new string[damageSkill.maxLevel];
        damageSkill.descriptions[0] = "damageSkill 1";
        damageSkill.descriptions[1] = "damageSkill 2";
        damageSkill.descriptions[2] = "damageSkill 3";
        damageSkill.prices = new int[damageSkill.maxLevel];
        damageSkill.prices[0] = 1800;
        damageSkill.prices[1] = 5600;
        damageSkill.prices[2] = 11000;
        
        // magical damage skill info
        magicalDamageSkill.maxLevel = 2;
        magicalDamageSkill.title = "Magical Damage Booster";
        magicalDamageSkill.descriptions = new string[magicalDamageSkill.maxLevel];
        magicalDamageSkill.descriptions[0] = "magicalDamageSkill 1";
        magicalDamageSkill.descriptions[1] = "magicalDamageSkill 2";
        magicalDamageSkill.prices = new int[magicalDamageSkill.maxLevel];
        magicalDamageSkill.prices[0] = 4000;
        magicalDamageSkill.prices[1] = 16000;
        
        // movement speed skill info
        movementSpeedSkill.maxLevel = 3;
        movementSpeedSkill.title = "Movement Speed Booster";
        movementSpeedSkill.descriptions = new string[movementSpeedSkill.maxLevel];
        movementSpeedSkill.descriptions[0] = "movementSpeedSkill 1";
        movementSpeedSkill.descriptions[1] = "movementSpeedSkill 2";
        movementSpeedSkill.descriptions[2] = "movementSpeedSkill 3";
        movementSpeedSkill.prices = new int[movementSpeedSkill.maxLevel];
        movementSpeedSkill.prices[0] = 2000;
        movementSpeedSkill.prices[1] = 8000;
        movementSpeedSkill.prices[2] = 19000;
        
        // jump skill info
        jumpSkill.maxLevel = 1;
        jumpSkill.title = "Triple Jump";
        jumpSkill.descriptions = new string[jumpSkill.maxLevel];
        jumpSkill.descriptions[0] = "jumpSkill 1";
        jumpSkill.prices = new int[jumpSkill.maxLevel];
        jumpSkill.prices[0] = 20000;
        
        infoCanvas = GameObject.Find("Info").transform.GetChild(2).gameObject;
    }

    public void OnClickButton()
    {
        var buttonName = EventSystem.current.currentSelectedGameObject.name;

        const int idx = 6;
        var skillName = buttonName.Substring(idx);
        var level = Int32.Parse(buttonName.Substring(buttonName.Length - 1));
        SkillJSON chosenSkill = null;

        if (buttonName.Contains("Armor"))
        {
            chosenSkill = armorSkill;
        }
        else if (buttonName.Contains("HP_"))
        {
            chosenSkill = hpRegenSkill;
        }
        else if (buttonName.Contains("HP")) 
        {
            chosenSkill = hpSkill;
        }
        else if (buttonName.Contains("Speed"))
        {
            chosenSkill = movementSpeedSkill;
        }
        else if (buttonName.Contains("MagicalDamage"))
        {
            chosenSkill = magicalDamageSkill;
        }
        else if (buttonName.Contains("Damage"))
        {
            chosenSkill = damageSkill;
        }
        else if (buttonName.Contains("Jump"))
        {
            chosenSkill = jumpSkill;
        }

        if (chosenSkill == null)
        {
            Debug.Log("Invalid button press");
            return;
        }

        var title = infoCanvas.transform.GetChild(0).gameObject;
        title.GetComponent<Text>().text = chosenSkill.title;

        var desc = infoCanvas.transform.GetChild(1).gameObject;
        desc.GetComponent<Text>().text = chosenSkill.descriptions[level - 1];

        var levelValue = infoCanvas.transform.GetChild(3).gameObject;
        levelValue.GetComponent<Text>().text = level + " / " + chosenSkill.maxLevel;

        var priceValue = infoCanvas.transform.GetChild(5).gameObject;
        priceValue.GetComponent<Text>().text = chosenSkill.prices[level - 1].ToString();

        var skills = infoCanvas.transform.GetChild(6).gameObject;
        var skill = skills.transform.Find(skillName.Substring(0, skillName.Length - 1)).gameObject;
        
        foreach (Transform child in skills.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        skill.SetActive(true);
    }
}
