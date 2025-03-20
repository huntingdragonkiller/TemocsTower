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
    int purchaseCost;
    public int PurchaseCost { get => purchaseCost; set => purchaseCost = value; }

    [SerializeField]
    int upgradeCost;
    public int UpgradeCost { get => upgradeCost; set => upgradeCost = value; }

    [SerializeField]
    string description;
    public string Description { get => description; set => description = value; }
    [SerializeField]
    Sprite sprite;
    public Sprite Sprite { get => sprite;}
}
