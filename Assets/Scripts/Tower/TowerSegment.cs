using UnityEngine;
using UnityEngine.Serialization;

public class TowerSegment : MonoBehaviour
{
    [Header("Tower Data")]
    public TowerScriptableObject towerData;
    private TowerManager towerManager;

    [Header("Attachments")]
    public Transform attachmentPoint;
    public TowerSegment belowSegment;
    
    // current stats
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    
    void Awake()
    {
        currentHealth = towerData.MaxHealth;
        currentDamage = towerData.Damage;
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

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log("My health is: " + currentHealth);
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        towerManager.SendMessage("DestroySegment", this);
    }
}
