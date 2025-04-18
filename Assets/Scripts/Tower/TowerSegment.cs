using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class TowerSegment : MonoBehaviour
{
    [Header("Tower Data")]
    public TowerScriptableObject towerData;
    TowerUIManager towerUIManager;
    private TowerManager towerManager;

    [Header("Attachments")]
    public Transform attachmentPoint;
    public TowerSegment belowSegment;
    
    // current stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    [HideInInspector]
    public int upgradeCost;

    public int currentLevel = 1;
    protected float maxHealth;
    protected float localTimeScale = 1f;
    HealthBarManager healthBar;
    private float warningPercent = .2f;
    bool isVisible = false;
    private static bool warningIssued;
    private static GameObject currentWarnedObject;
    private static readonly int IsBuilding = Animator.StringToHash("IsBuilding");

    private Animator anim;
    private bool isBuilding;
    private float buildTime = 15 / 60f;

    public virtual void Awake()
    {
        towerUIManager = GetComponentInChildren<TowerUIManager>();
        healthBar = GetComponentInChildren<HealthBarManager>();
        maxHealth = towerData.MaxHealth;
        currentHealth = maxHealth;
        currentDamage = towerData.Damage;
        upgradeCost = towerData.UpgradeCost;
        towerUIManager.UpdateHealth(currentHealth, maxHealth);
        towerUIManager.UpdateCost(upgradeCost);
        towerUIManager.UpdateLevel(currentLevel);
        Debug.Log("My upgrade costs: " + upgradeCost);
    }

    // Changed this
    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        isBuilding = true;
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
    }

    void Update()
    {
        buildTime -= Time.deltaTime;
        if (buildTime <= 0)
        {
            isBuilding = false;
            anim.SetBool(IsBuilding, isBuilding);
        }
    }
    
    void OnDestroy()
    {
        StopAllCoroutines();        
    }

    public void SetLocalTimeScale(float toSet = 1f){
        Debug.Log(gameObject.name + " got buffed");
        localTimeScale = toSet;
    }


    public void AttachTo(TowerSegment lowerSegment)
    {
        if (lowerSegment != null)
        {
            belowSegment = lowerSegment;
            transform.position = lowerSegment.attachmentPoint.position;
        }
    }

    public void TryUpgrade(){
        Upgrade();
    }
    public virtual bool Upgrade()
    {
        if (FindFirstObjectByType<CoinManager>().SpendCoins(upgradeCost))
        {
            currentLevel++;
            towerUIManager.UpdateLevel(currentLevel);
            return true;
        } else
        {
            return false;
            //if not enough money returns, this lets us inherit this function in our other towers
            // and add only code for if the upgrade was successful
        }
    }
    
    public void FullHeal(){
        currentHealth = maxHealth;
        towerUIManager.UpdateHealth(currentHealth, maxHealth);
        healthBar.UpdateHealth(1);
    }


    public void HealDamage(float healAmount){
        currentHealth = Math.Clamp(currentHealth + healAmount, 0, maxHealth);
        towerUIManager.UpdateHealth(currentHealth, maxHealth);
        healthBar.UpdateHealth(currentHealth/maxHealth);
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        // Debug.Log("My health is: " + currentHealth);
        if (currentHealth <= 0)
        {
            Kill();
        }
        CheckForWarning();
        towerUIManager.UpdateHealth(currentHealth, maxHealth);
        healthBar.UpdateHealth(currentHealth/maxHealth);
    }

    private void CheckForWarning()
    {
        if (currentHealth / maxHealth < warningPercent && !isVisible){
            if(!warningIssued){
                warningIssued = true;
                currentWarnedObject = gameObject;
                WarningUIManager.instance.ShowWarning(gameObject);
            }
            
        }
    }

    public void ResetWarning()
    {
        warningIssued = false;
        currentWarnedObject = null;
    }

    void OnBecameVisible()
    {
        isVisible = true;
    }

    void OnBecameInvisible()
    {
        isVisible = false;
    }
    public virtual void Kill()
    {
        towerManager.SendMessage("DestroySegment", this);
    }
}
