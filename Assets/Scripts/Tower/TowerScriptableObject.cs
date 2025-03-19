using UnityEngine;

[CreateAssetMenu(fileName = "TowerScriptableObject", menuName = "ScriptableObjects/Tower")]
public class TowerScriptableObject : ScriptableObject
{
    //Base stats for towers
    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    [SerializeField]
    float damage;
    public float Damage { get => damage; set => damage = value; }

    [SerializeField]
    int upgradeCost;
    public int UpgradeCost { get => upgradeCost; set => upgradeCost = value; }
}
