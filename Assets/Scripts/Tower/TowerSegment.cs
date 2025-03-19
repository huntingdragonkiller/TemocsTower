using UnityEngine;
using UnityEngine.Serialization;

public class TowerSegment : MonoBehaviour
{
    [Header("Tower Data")]
    public TowerScriptableObject towerData;
    public TowerUIManager towerUIManager;
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
    private float maxHealth;

    public virtual void Awake()
    {
        maxHealth = towerData.MaxHealth;
        currentHealth = maxHealth;
        currentDamage = towerData.Damage;
        upgradeCost = towerData.UpgradeCost;
        towerUIManager.UpdateHealth(currentHealth, maxHealth);
        towerUIManager.UpdateCost(upgradeCost);
        towerUIManager.UpdateLevel(currentLevel);
        Debug.Log("My upgrade costs: " + upgradeCost);
    }

    void Start()
    {
        towerManager = GameObject.Find("TowerManager").GetComponent<TowerManager>();
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

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log("My health is: " + currentHealth);
        if (currentHealth <= 0)
        {
            Kill();
        }
        towerUIManager.UpdateHealth(currentHealth, maxHealth);
    }

    public void Kill()
    {
        towerManager.SendMessage("DestroySegment", this);
    }
}
