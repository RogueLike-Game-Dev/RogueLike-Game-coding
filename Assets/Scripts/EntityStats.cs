using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    [SerializeField] private int _maxHP = 100;
    private int _currentHP;
    [SerializeField] private int _gold;
    public int maxHP {
        get { return _maxHP; }
        set
        {
            if (_maxHP == value) return;
            _maxHP = value;
            OnMaxHPChange?.Invoke();
        }
    }
    [HideInInspector] public int currentHP
    {
        get { return _currentHP; }
        private set
        {
            if (_currentHP == value) return;
            _currentHP = value;
            OnHPChange?.Invoke();
        }
    }
    public int gold //For player: how much gold he has, for enemies, how much gold they drop
    {
        get { return _gold; }
        set
        {
            if (_gold == value) return;
            _gold = value;
            OnGoldChange?.Invoke();
        }
    }
    [Tooltip("How much time passes between attacks")] public float timeBetweenAttacks;
    [Tooltip("How much DMG does the entity does")] public int DMG;
    [Tooltip("How fast does the entity move")] public float movementSpeed;
    [Tooltip("How much should the player get knocked back when colliding with this entity")] public float knockBackStrength; 
    [SerializeField] private Animator animator;
    [SerializeField] [Tooltip("Name of the Animator Trigger for Hurt animation")] private string hurtTriggerKey = "isHurt";
    [SerializeField] [Tooltip("Name of the Animator Trigger for Dying animation")] private string deathTriggerKey = "isDying";
    #region Events and Delegates
    public delegate void OnHPChangeDelegate();
    public delegate void OnMaxHPChangeDelegate();
    public delegate void OnGoldChangeDelegate();
    public event OnHPChangeDelegate OnHPChange;
    public event OnMaxHPChangeDelegate OnMaxHPChange;
    public event OnGoldChangeDelegate OnGoldChange;
    #endregion
    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>(); //Try to get component if forgot to set
        currentHP = maxHP;
        OnHPChange += EntityStats_OnHPChange;
    }

    private void EntityStats_OnHPChange()
    {
        if (currentHP <= 0) //If it's supposed to die
        {
            if (animator != null && AnimatorHasParameter(animator,deathTriggerKey)) //And has animations + death animation
            {

                animator.SetTrigger(deathTriggerKey); //Trigger the animation and supply function for the event
            }
            else //No animations set, just make it inactive
            {
                   if (this.gameObject.name == "Player")
                GameManager.EndRun();

                this.gameObject.SetActive(false);
            }
         
        }
    }
    private bool AnimatorHasParameter(Animator animator, string parameterName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == parameterName)
                return true;
        }
        return false;
    }
    
    public void OnDeath() //it's handler function for animation event
    {
        if (this.gameObject.name == "Player")
            GameManager.EndRun();
        this.gameObject.SetActive(false);
    }
    public void Heal(int amount)
    {
        if (amount < 0)
        {
            Debug.Log("NU ABUZA DE FUNCTIE");
            return;
        }
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }
    public void Damage(int amount)
    {
        if (amount < 0)
        {
            Debug.Log("NU ABUZA DE FUNCTIE");
            return;
        }
        currentHP -= amount;
        if (animator != null)
        {
            Debug.Log("Called animator key");
            animator.SetTrigger(hurtTriggerKey);
        }
    }
}
