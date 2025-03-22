using UnityEngine;
using UnityEngine.Audio;

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
    public float maxHealth;
    [HideInInspector]
    public float currentDamage;
    [HideInInspector]
    public int spawnCost;
    [HideInInspector]
    public bool isGroundEnemy;
    [HideInInspector]
    public int killReward;
    [HideInInspector]
    public AudioResource attackSoundClip;
    [HideInInspector]
    public AudioResource damageSoundClip;
    [HideInInspector]
    public AudioResource deathSoundClip;
    public HealthBarManager healthBar;

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBarManager>();
        InitializeValues();
        currentHealth = maxHealth;
    }

    public void InitializeValues(float percentChange = 1){
        currentMoveSpeed = enemyData.MoveSpeed * percentChange * Time.fixedDeltaTime;//Gives us the move speed in tiles per second
        maxHealth = enemyData.MaxHealth * percentChange;
        currentDamage = enemyData.Damage * percentChange;
        attackSpeed = enemyData.AttackSpeed * (1 / percentChange);
        isGroundEnemy = enemyData.IsGroundEnemy;
        spawnCost = enemyData.SpawnCost;
        killReward = enemyData.KillReward;
        attackSoundClip = enemyData.AttackSoundClip;
        damageSoundClip = enemyData.DamageSoundClip;
        deathSoundClip = enemyData.DeathSoundClip;
    }
    
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        healthBar.UpdateHealth(currentHealth/enemyData.MaxHealth);
        if (currentHealth <= 0)
        {
            Kill();
        } else {
            //play the damage sound instead of the death sound
            SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1f);
        }
    }

    public void Kill()
    {
        SoundFXManager.instance.PlaySoundFXClip(deathSoundClip, transform, 1f);
        FindFirstObjectByType<CoinManager>().AddCoins(killReward);
        Destroy(gameObject);
    }
}
