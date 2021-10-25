using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Text hpDisplay;
    [SerializeField] private Text goldDisplay;

    public Gradient hpGradient;

    private EntityStats playerStats;
    private float curValue;
    private float targetValue;
    private float fillSpeed=40f;

    // Start is called before the first frame update
    private void Awake()
    {
        //Get reference and set initial values
        playerStats = GameObject.Find("Player").GetComponent<EntityStats>();

        //Subscribe to events
        playerStats.OnHPChange += PlayerStats_OnHPChange;
        playerStats.OnMaxHPChange += PlayerStats_OnMaxHPChange;
        playerStats.OnGoldChange += PlayerStats_OnGoldChange;

    }
    private void Start()
    {
        slider.maxValue = playerStats.maxHP;
        slider.value = playerStats.currentHP;
        goldDisplay.text = playerStats.gold.ToString();
        hpDisplay.text = playerStats.currentHP.ToString() + " / " + playerStats.maxHP.ToString();
        targetValue = curValue = slider.value;
    }
    private void Update()
    {   
        //To make HPSlider smooth
        curValue = Mathf.MoveTowards(curValue, targetValue, Time.deltaTime * fillSpeed);
        slider.value = curValue;
    }
    #region Event Delegates 
    private void PlayerStats_OnGoldChange() { goldDisplay.text = playerStats.gold.ToString(); }

    private void PlayerStats_OnMaxHPChange()
    {
        slider.maxValue = playerStats.maxHP;
        hpDisplay.text = playerStats.currentHP.ToString() + " / " + playerStats.maxHP.ToString();
        fill.color = hpGradient.Evaluate(slider.normalizedValue);  
    }

    private void PlayerStats_OnHPChange()
    {
        targetValue = playerStats.currentHP;
        hpDisplay.text = playerStats.currentHP.ToString() + " / " + playerStats.maxHP.ToString();
        fill.color = hpGradient.Evaluate(slider.normalizedValue);
    }
    #endregion
}
