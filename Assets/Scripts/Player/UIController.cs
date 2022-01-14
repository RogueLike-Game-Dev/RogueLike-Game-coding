using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Text hpDisplay;
    [SerializeField] private Text goldDisplay;
    [SerializeField] private Text armorDisplay;
    [SerializeField] private Text immunityDisplay;
    [SerializeField] private Text invisibilityDisplay;
    [SerializeField] private Text immunityKey;
    [SerializeField] private Text invisibilityKey;

    public Gradient hpGradient;

    private EntityStats playerStats;
    private float curValue;
    private float targetValue;
    private float fillSpeed = 40f;

    private PurchasedItems purchasedItems;

    // Start is called before the first frame update
    private void Awake()
    {
        // Get reference and set initial values
        playerStats = GameObject.Find("Player").GetComponent<EntityStats>();

        // Subscribe to events
        playerStats.OnHPChange += PlayerStats_OnHPChange;
        playerStats.OnMaxHPChange += PlayerStats_OnMaxHPChange;
        playerStats.OnGoldChange += PlayerStats_OnGoldChange;
        playerStats.OnArmorChange += PlayerStats_OnArmorChange;

        immunityKey.text = System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Item1", "Alpha1")).ToString();
        invisibilityKey.text = System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Item2", "Alpha2")).ToString();
    }
    
    private void Start()
    {
        purchasedItems = PurchasedItems.getInstance();
        
        slider.maxValue = playerStats.maxHP;
        slider.value = playerStats.currentHP;
        goldDisplay.text = playerStats.gold.ToString();
        hpDisplay.text = playerStats.currentHP + " / " + playerStats.maxHP;
        armorDisplay.text = playerStats.currentArmor + " / " + playerStats.maxArmor;
        immunityDisplay.text = purchasedItems.immunityNr.ToString();
        invisibilityDisplay.text = purchasedItems.invisibilityNr.ToString();
        targetValue = curValue = slider.value;
    }
    private void Update()
    {
        if (Math.Abs(targetValue - curValue) > 100)
        {
            curValue = targetValue;
        }
        else
        {
            // To make HPSlider smooth
            curValue = Mathf.MoveTowards(curValue, targetValue, Time.deltaTime * fillSpeed);
        }
        slider.value = curValue;

        immunityDisplay.text = purchasedItems.immunityNr.ToString();
        invisibilityDisplay.text = purchasedItems.invisibilityNr.ToString();
    }
    
    #region Event Delegates
    private void PlayerStats_OnGoldChange() 
    {
        goldDisplay.text = playerStats.gold.ToString();
    }

    private void PlayerStats_OnMaxHPChange()
    {
        slider.maxValue = playerStats.maxHP;
        slider.value = playerStats.currentHP;
        hpDisplay.text = playerStats.currentHP + " / " + playerStats.maxHP;
        fill.color = hpGradient.Evaluate(slider.normalizedValue);  
    }

    private void PlayerStats_OnHPChange()
    {
        targetValue = playerStats.currentHP;
        hpDisplay.text = playerStats.currentHP + " / " + playerStats.maxHP;
        fill.color = hpGradient.Evaluate(slider.normalizedValue);
    }

    private void PlayerStats_OnArmorChange()
    {
        armorDisplay.text = playerStats.currentArmor + " / " + playerStats.maxArmor;
    }
    #endregion
}
