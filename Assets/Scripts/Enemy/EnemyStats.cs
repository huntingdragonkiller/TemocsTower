using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;
    
    //Current stats
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float attackSpeed;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentDamage;
    [HideInInspector]
    public int spawnCost;
    [HideInInspector]
    public bool isGroundEnemy;
    [HideInInspector]
    public int killReward;
    public HealthBarManager healthBar;

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBarManager>();
        currentMoveSpeed = enemyData.MoveSpeed * Time.fixedDeltaTime;//Gives us the move speed in tiles per second
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
        attackSpeed = enemyData.AttackSpeed;
        isGroundEnemy = enemyData.IsGroundEnemy;
        spawnCost = enemyData.SpawnCost;
        killReward = enemyData.KillReward;
    }
    
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        healthBar.UpdateHealth(currentHealth/enemyData.MaxHealth);
        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        FindFirstObjectByType<CoinManager>().AddCoins(killReward);
        Destroy(gameObject);
    }
}
