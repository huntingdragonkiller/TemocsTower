using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    //Base stats for enemies
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

    [SerializeField]
    float attackSpeed;
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }

    [SerializeField]
    float maxHealth;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    
    [SerializeField]
    float damage;
    public float Damage { get => damage; set => damage = value; }

    [SerializeField]
    bool isGroundEnemy;
    public bool IsGroundEnemy { get => isGroundEnemy; set => isGroundEnemy = value; }
    [SerializeField]
    int spawnCost;
    public int SpawnCost { get => spawnCost; set => spawnCost = value; }
    [SerializeField]
    int killReward;
    public int KillReward { get => killReward; set => killReward = value; }
    [SerializeField]
    AudioResource attackSoundClip;
    public AudioResource AttackSoundClip { get => attackSoundClip; set => attackSoundClip = value; }
    [SerializeField]
    AudioResource damageSoundClip;
    public AudioResource DamageSoundClip { get => damageSoundClip; set => damageSoundClip = value; }
    [SerializeField]
    AudioResource deathSoundClip;
    public AudioResource DeathSoundClip { get => deathSoundClip; set => deathSoundClip = value; }

}
