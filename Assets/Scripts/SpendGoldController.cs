using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillJSON
{
    public int maxLevel;
    public string title;
    public string[] descriptions;
    public int[] prices;
    public bool[] owned;
}

public class SpendGoldController : MonoBehaviour
{
    // private const int nrButtons = 18;
    // [SerializeField] public Button[] buttons = new Button[nrButtons];

    private int playerGold = 1000000000;     // must update this when player finished a run (by adding the gold collected in that run)
                                            // OR getting it from the save file (when player loads the game)

    private GameObject infoCanvas;
    private SkillJSON chosenSkill;
    private string buttonName;
    private bool canBuy;
    private bool canPress = true;
    private Text buyInfo;
    private Color originalTextColor;
    private GameObject buttonsContainer;

    private string[] armorButtonsInOrder;
    private string[] hpButtonsInOrder;
    private string[] damageButtonsInOrder;
    private string[] speedButtonsInOrder;
    private int armorIndex;
    private int hpIndex;
    private int damageIndex;
    private int speedIndex;
    
    #region skillobjects
    private SkillJSON armorSkill;
    private SkillJSON hpRegenSkill;
    private SkillJSON hpSkill;
    private SkillJSON damageSkill;
    private SkillJSON magicalDamageSkill;
    private SkillJSON movementSpeedSkill;
    private SkillJSON jumpSkill;
    #endregion

    // TODO: change the titles and descriptions of each skill
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
        armorSkill.owned = new[] {false, false, false};

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
        hpRegenSkill.owned = new[] {false, false, false};

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
        hpSkill.owned = new[] {false, false, false};

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
        damageSkill.owned = new[] {false, false, false};

        // magical damage skill info
        magicalDamageSkill.maxLevel = 2;
        magicalDamageSkill.title = "Magical Damage Booster";
        magicalDamageSkill.descriptions = new string[magicalDamageSkill.maxLevel];
        magicalDamageSkill.descriptions[0] = "magicalDamageSkill 1";
        magicalDamageSkill.descriptions[1] = "magicalDamageSkill 2";
        magicalDamageSkill.prices = new int[magicalDamageSkill.maxLevel];
        magicalDamageSkill.prices[0] = 4000;
        magicalDamageSkill.prices[1] = 16000;
        magicalDamageSkill.owned = new[] {false, false};

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
        movementSpeedSkill.owned = new[] {false, false, false};

        // jump skill info
        jumpSkill.maxLevel = 1;
        jumpSkill.title = "Triple Jump";
        jumpSkill.descriptions = new string[jumpSkill.maxLevel];
        jumpSkill.descriptions[0] = "jumpSkill 1";
        jumpSkill.prices = new int[jumpSkill.maxLevel];
        jumpSkill.prices[0] = 20000;
        jumpSkill.owned = new[] {false};

        infoCanvas = GameObject.Find("Info").transform.GetChild(2).gameObject;
        buyInfo = infoCanvas.transform.Find("BuyInfo").gameObject.GetComponent<Text>();
        buyInfo.text = "";
        originalTextColor = buyInfo.color;

        buttonsContainer = GameObject.Find("BUTTONS");

        armorButtonsInOrder = new string[3];
        hpButtonsInOrder = new string[6];
        damageButtonsInOrder = new string[5];
        speedButtonsInOrder = new string[4];
        InitializeButtonsOrder(0, 3, armorButtonsInOrder);
        InitializeButtonsOrder(3, 9, hpButtonsInOrder);
        InitializeButtonsOrder(9, 14, damageButtonsInOrder);
        InitializeButtonsOrder(14, 18, speedButtonsInOrder);
    }

    void InitializeButtonsOrder(int start, int end, string[] arr)
    {
        for (int i = start; i < end; i++)
        {
            var child = buttonsContainer.transform.GetChild(i);
            arr[i - start] = child.gameObject.name;
        }
    }

    private void Update()
    {
        if (chosenSkill != null) // if a skill has been selected
        {
            var level = Int32.Parse(buttonName.Substring(buttonName.Length - 1)) - 1;
            var price = chosenSkill.prices[level];

            if (chosenSkill.owned[level] == false) // if the chosen skill wasn't bought yet
            {
                if (price > playerGold)
                {
                    buyInfo.text = "Not enough gold";
                    canBuy = false;
                }
                else
                {
                    buyInfo.text = "Press Enter to buy";
                    canBuy = true;
                }

                if (canPress)
                {
                    buyInfo.color = originalTextColor;
                }

                if (Input.GetKeyDown(KeyCode.Return) && canPress)
                {
                    if (canBuy)
                    {
                        playerGold -= chosenSkill.prices[level];
                        chosenSkill.owned[level] = true;

                        if (chosenSkill == armorSkill)
                        {
                            armorIndex++;
                            UnlockNextSkillLevel(armorButtonsInOrder, armorIndex, "ARMOR_TREE", 3);
                        }
                        else if (chosenSkill == hpSkill || chosenSkill == hpRegenSkill)
                        {
                            hpIndex++;
                            UnlockNextSkillLevel(hpButtonsInOrder, hpIndex, "HP_TREE", 6);
                        }
                        else if (chosenSkill == damageSkill || chosenSkill == magicalDamageSkill)
                        {
                            damageIndex++;
                            UnlockNextSkillLevel(damageButtonsInOrder, damageIndex, "DAMAGE_TREE", 5);
                        }
                        else if (chosenSkill == movementSpeedSkill || chosenSkill == jumpSkill)
                        {
                            speedIndex++;
                            UnlockNextSkillLevel(speedButtonsInOrder, speedIndex, "SPEED_JUMP_TREE", 4);
                        }
                    }
                    else
                    {
                        StartCoroutine(ChangeInfoStyle());
                    }
                }
            }
            else
            {
                buyInfo.text = "Owned";
                buyInfo.color = new Color(0.0f, 0.53f, 0.4f);
            }
        }
    }

    private IEnumerator ChangeInfoStyle()
    {
        var oldFont = buyInfo.fontSize;
        
        buyInfo.fontSize = oldFont + 3;
        buyInfo.color = new Color(0.72f, 0.0f, 0.0f);
        canPress = false;

        yield return new WaitForSeconds(0.7f);
        
        buyInfo.fontSize = oldFont;
        buyInfo.color = originalTextColor;
        canPress = true;
    }

    private void UnlockNextSkillLevel(string[] buttons, int index, string treeName, int maxIndex)
    {
        var tree = GameObject.Find(treeName);

        var outline = tree.transform.Find("Outline" + index);
        if (outline)
        {
            outline.gameObject.SetActive(true);
        }        
        
        if (index >= buttons.Length)
        {
            return;
        }

        // activate next level button
        var newButton = buttonsContainer.transform.Find(buttons[index]);
        if (newButton != null)
        {
            newButton.gameObject.SetActive(true);
        }

        // deactivate next locked (question mark) skill
        
        // index = the index of the locked skill that must be unlocked
        // - 1 = the locked skill that must be deactivated
        // + buttons.Length = jump over the height of the tree
        if (index - 1 + buttons.Length < maxIndex)
        {
            var locked = tree.transform.GetChild(index - 1 + buttons.Length);
            locked.gameObject.SetActive(false);
        }

        // activate next skill
        if (index < maxIndex)
        {
            var skill = tree.transform.GetChild(index);
            skill.gameObject.SetActive(true);
        }
        
        // activate next+1 locked (question mark)
        if (index + buttons.Length < maxIndex)
        {
            var unlocked = tree.transform.GetChild(index + buttons.Length);
            unlocked.gameObject.SetActive(true);
        }
    }

    public void OnClickButton()
    {
        buttonName = EventSystem.current.currentSelectedGameObject.name;

        const int idx = 6;
        var skillName = buttonName.Substring(idx);
        var level = Int32.Parse(buttonName.Substring(buttonName.Length - 1)) - 1;

        // get the type of skill selected
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

        // setting information in the panel according to the selected type of skill
        var title = infoCanvas.transform.GetChild(0).gameObject;
        title.GetComponent<Text>().text = chosenSkill.title;

        var desc = infoCanvas.transform.GetChild(1).gameObject;
        desc.GetComponent<Text>().text = chosenSkill.descriptions[level];

        var levelValue = infoCanvas.transform.GetChild(3).gameObject;
        levelValue.GetComponent<Text>().text = level + 1 + " / " + chosenSkill.maxLevel;

        var priceValue = infoCanvas.transform.GetChild(5).gameObject;
        priceValue.GetComponent<Text>().text = chosenSkill.prices[level].ToString();

        var skills = infoCanvas.transform.GetChild(6).gameObject;
        var skill = skills.transform.Find(skillName.Substring(0, skillName.Length - 1)).gameObject;
        
        // activate the selected type of skill and deactivate the rest
        foreach (Transform child in skills.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        skill.SetActive(true);
    }
}
