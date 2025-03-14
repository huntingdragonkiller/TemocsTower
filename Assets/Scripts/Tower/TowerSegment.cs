using UnityEngine;

public class TowerSegment : MonoBehaviour
{
    [Header("Tower Data")]
    public TowerScriptableObject towerData;

    [Header("Attatchments")]
    public Transform attatchmentPoint;
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

    public void AttatchTo(TowerSegment lowerSegment)
    {
        if (lowerSegment != null)
        {
            belowSegment = lowerSegment;
            transform.position = lowerSegment.attatchmentPoint.position;
        }
    }
}
