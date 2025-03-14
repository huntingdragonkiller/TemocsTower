using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    //Base stats for enemies
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    
    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    
    [SerializeField]
    float damage;
    public float Damage { get => damage; set => damage = value; }
}
