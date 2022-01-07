using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillJSON
{
    public int maxLevel;
    public string[] titles;
    public string[] descriptions;
    public int[] prices;
    public bool[] owned;
    public int number;
    public string type = "skill";
}

public class SpendGoldController : MonoBehaviour
{
    private PurchasedItems purchasedItems;
    
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
    private SkillJSON reviveItem;
    private SkillJSON immunityItem;
    private SkillJSON invisibilityItem;
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
        
        // initializing items
        reviveItem = new SkillJSON();
        immunityItem = new SkillJSON();
        invisibilityItem = new SkillJSON();

        // armor skill info
        armorSkill.maxLevel = 3;
        armorSkill.titles = new[] {"Chainmail", "Cuirass", "Full Suit"};
        armorSkill.descriptions = new[]
        {
            "Put on some armor to better protect yourself from foes and the damage they deal to you. " +
            "Armor does not regenerate, but if it breaks, you will recieve half the damage from that hit.", 
            "Put on some MORE armor to better protect yourself from stronger enemies. " +
            "Armor does not regenerate, but if it breaks, you will recieve half the damage from that hit.", 
            "Put on ALL OF THE armor to protect yourself from the strongest enemies you will encounter. " +
            "Armor does not regenerate, but if it breaks, you will recieve half the damage from that hit."
        };
        armorSkill.prices = new[] {1000, 5000, 15000};
        armorSkill.owned = new[] {false, false, false};

        // hp regen skill info
        hpRegenSkill.maxLevel = 3;
        hpRegenSkill.titles = new[] {"Earth", "Moon", "Sun"};
        hpRegenSkill.descriptions = new[]
        {
            "Discover the secrets of the EARTH to be able to regenerate lost health and extend your longevity in adventuring.", 
            "Discover the secrets of the MOON to be able to regenerate lost health and extend your longevity in adventuring.", 
            "Discover the secrets of the SUN to be able to regenerate lost health and extend your longevity in adventuring."
        };
        hpRegenSkill.prices = new[] {2000, 10000, 20000};
        hpRegenSkill.owned = new[] {false, false, false};

        // hp skill info
        hpSkill.maxLevel = 3;
        hpSkill.titles = new[] {"Exercise", "Sports", "Athletics"};
        hpSkill.descriptions = new[]
        {
            "Start moving your body to become HEALTHY, to be able to withstand more damage from your enemies and to survive longer.",
            "Start moving your body to become HEALTHIER, to be able to withstand more damage from your enemies and to survive longer.",
            "Start moving your body to become THE HEALTIEST, to be able to withstand more damage from your enemies and to survive longer."
        };
        hpSkill.prices = new[] {1500, 5000, 12000};
        hpSkill.owned = new[] {false, false, false};

        // damage skill info
        damageSkill.maxLevel = 3;
        damageSkill.titles = new[] {"Knife", "Sword", "Zweihander"};
        damageSkill.descriptions = new[]
        {
            "Upgrade your weapon so that your attacks deal MORE damage and your enemies fall faster.",
            "Upgrade your weapon so that your attacks deal EVEN MORE damage and your enemies fall faster.",
            "Upgrade your weapon so that your attacks deal THE MOST damage and your enemies fall faster."
        };
        damageSkill.prices = new[] {1800, 5600, 11000};
        damageSkill.owned = new[] {false, false, false};

        // magical damage skill info
        magicalDamageSkill.maxLevel = 2;
        magicalDamageSkill.titles = new[] {"Better Abilities", "Awesome Abilities"};
        magicalDamageSkill.descriptions = new[]
        {
            "The magic of the stars imbudes you. With their help, now, your special attacks deal MORE damage to help you defeat strong foes.",
            "The magic of the stars imbudes you. With their help, now, your special attacks deal THE MOST damage to help you defeat the strongest foes."
        };
        magicalDamageSkill.prices = new[] {4000, 16000};
        magicalDamageSkill.owned = new[] {false, false};

        // movement speed skill info
        movementSpeedSkill.maxLevel = 3;
        movementSpeedSkill.titles = new[] {"Socks", "Shoes", "Boots"};
        movementSpeedSkill.descriptions = new[]
        {
            "Upgrade your footware to be able to move FASTER to take your enemies by surprise and escape their attacks.",
            "Upgrade your footware to be able to move THE FASTEST to take your enemies by surprise and escape their attacks.",
            "Upgrade your footware to be able to move LIKE A LIGHTNING to take your enemies by surprise and escape their attacks."
        };
        movementSpeedSkill.prices = new[] {2000, 8000, 19000};
        movementSpeedSkill.owned = new[] {false, false, false};

        // jump skill info
        jumpSkill.maxLevel = 1;
        jumpSkill.titles = new[] {"Triple Jump"};
        jumpSkill.descriptions = new[]
        {
            "After years of trainning, you will now be able to jump EVEN HIGHER, letting you reach higher places."
        };
        jumpSkill.prices = new[] {20000};
        jumpSkill.owned = new[] {false};

        infoCanvas = GameObject.Find("Info").transform.GetChild(2).gameObject;
        buyInfo = infoCanvas.transform.Find("BuyInfo").gameObject.GetComponent<Text>();
        buyInfo.text = "";
        originalTextColor = buyInfo.color;

        reviveItem.titles = new [] {"Revive!"};
        reviveItem.descriptions = new[]
        {
            "Just like the legendary bird, the Phoenix, you can revive yourself once, so that your adventure is never cut short."
        };
        reviveItem.number = 0;
        reviveItem.prices = new[] {10000};
        reviveItem.owned = new[] {false};
        reviveItem.type = "item";

        immunityItem.titles = new [] {"Invulnerability!"};
        immunityItem.descriptions = new[]
        {
            "For a short time, the Gods smile on you and protect you from any type of harm. But don't count too much on their help, " +
            "they quit helping very fast."
        };
        immunityItem.number = 0;
        immunityItem.prices = new[] {6000};
        immunityItem.owned = new[] {false};
        immunityItem.type = "item";
        
        invisibilityItem.titles = new [] {"Invisibility!"};
        invisibilityItem.descriptions = new[]
        {
            "Learn the ways of the shadows and turn yourself invisible for a short amount of time, evading enemies and attacks."
        };
        invisibilityItem.number = 0;
        invisibilityItem.prices = new[] {14000};
        invisibilityItem.owned = new[] {false};
        invisibilityItem.type = "item";

        buttonsContainer = GameObject.Find("BUTTONS");

        armorButtonsInOrder = new string[3];
        hpButtonsInOrder = new string[6];
        damageButtonsInOrder = new string[5];
        speedButtonsInOrder = new string[4];
        InitializeButtonsOrder(0, 3, armorButtonsInOrder);
        InitializeButtonsOrder(3, 9, hpButtonsInOrder);
        InitializeButtonsOrder(9, 14, damageButtonsInOrder);
        InitializeButtonsOrder(14, 18, speedButtonsInOrder);

        purchasedItems = PurchasedItems.getInstance();
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
                            purchasedItems.armorMaxLevel = armorIndex;
                            UnlockNextSkillLevel(armorButtonsInOrder, armorIndex, "ARMOR_TREE", 3);
                        }
                        else if (chosenSkill == hpSkill || chosenSkill == hpRegenSkill)
                        {
                            hpIndex++;
                            purchasedItems.hpMaxLevel = hpIndex;
                            UnlockNextSkillLevel(hpButtonsInOrder, hpIndex, "HP_TREE", 6);
                        }
                        else if (chosenSkill == damageSkill || chosenSkill == magicalDamageSkill)
                        {
                            damageIndex++;
                            purchasedItems.damageMaxLevel = damageIndex;
                            UnlockNextSkillLevel(damageButtonsInOrder, damageIndex, "DAMAGE_TREE", 5);
                        }
                        else if (chosenSkill == movementSpeedSkill || chosenSkill == jumpSkill)
                        {
                            speedIndex++;
                            purchasedItems.speedMaxLevel = speedIndex;
                            UnlockNextSkillLevel(speedButtonsInOrder, speedIndex, "SPEED_JUMP_TREE", 4);
                        }
                        else if (chosenSkill == reviveItem || chosenSkill == immunityItem ||
                                 chosenSkill == invisibilityItem)
                        {
                            chosenSkill.number++;
                            chosenSkill.owned[level] = false;

                            if (chosenSkill == reviveItem)
                            {
                                purchasedItems.reviveNr = chosenSkill.number;
                            }
                            else if (chosenSkill == immunityItem)
                            {
                                purchasedItems.immunityNr = chosenSkill.number;
                            }
                            else if (chosenSkill == invisibilityItem)
                            {
                                purchasedItems.invisibilityNr = chosenSkill.number;
                            }
                            
                            var levelText = infoCanvas.transform.GetChild(2).gameObject;
                            var levelValue = infoCanvas.transform.GetChild(3).gameObject;
                            levelText.GetComponent<Text>().text = "OWNED";
                            levelValue.GetComponent<Text>().text = chosenSkill.number.ToString();
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
                if (chosenSkill.type != "item")
                {
                    buyInfo.text = "Owned";
                    buyInfo.color = new Color(0.0f, 0.53f, 0.4f);
                }
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
        else if (buttonName.Contains("Revive"))
        {
            chosenSkill = reviveItem;
        }
        else if (buttonName.Contains("Immunity"))
        {
            chosenSkill = immunityItem;
        }
        else if (buttonName.Contains("Invisibility"))
        {
            chosenSkill = invisibilityItem;
        } 

        if (chosenSkill == null)
        {
            Debug.Log("Invalid button press");
            return;
        }

        // setting information in the panel according to the selected type of skill
        var title = infoCanvas.transform.GetChild(0).gameObject;
        title.GetComponent<Text>().text = chosenSkill.titles[level];

        var desc = infoCanvas.transform.GetChild(1).gameObject;
        desc.GetComponent<Text>().text = chosenSkill.descriptions[level];
        
        var levelText = infoCanvas.transform.GetChild(2).gameObject;
        var levelValue = infoCanvas.transform.GetChild(3).gameObject;
        if (chosenSkill.type == "item")
        {
            levelText.GetComponent<Text>().text = "OWNED";
            levelValue.GetComponent<Text>().text = chosenSkill.number.ToString();
        }
        else
        {
            levelText.GetComponent<Text>().text = "LEVEL";
            levelValue.GetComponent<Text>().text = level + 1 + " / " + chosenSkill.maxLevel;
        }

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
