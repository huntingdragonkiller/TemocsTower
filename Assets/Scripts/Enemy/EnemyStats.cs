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
    private bool dying = false;
    private static readonly int TakeDamageTrigger = Animator.StringToHash("TakeDamage");
    private static readonly int DieTrigger = Animator.StringToHash("Die");

    private Animator anim;

    void Awake()
    {
        healthBar = GetComponentInChildren<HealthBarManager>();
        InitializeValues();
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void InitializeValues(float percentChange = 1f){
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
        //This is to prevent a situation where something is getting attacked so often
        //that the animation to die keeps resetting before it actually dies
        if(dying)
        {
            return;
        }
        currentHealth -= dmg;
        healthBar.UpdateHealth(currentHealth/enemyData.MaxHealth);
        if (currentHealth <= 0)
        {
            Die();
        } else {
            //play the damage sound instead of the death sound
            SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1f);
            anim.SetTrigger(TakeDamageTrigger);
        }
    }

    public void ResetDamageTrigger(){
        anim.ResetTrigger(TakeDamageTrigger);
    }

    public void Die()
    {
        dying = true;
        SoundFXManager.instance.PlaySoundFXClip(deathSoundClip, transform, 1f);
        FindFirstObjectByType<CoinManager>().AddCoins(killReward);
        anim.SetTrigger(DieTrigger);
    }

    //This is called within the animation clips for the enemies death
    public void Kill(){
        Destroy(gameObject);
    }
}
