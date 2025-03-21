using System.Collections;
using UnityEngine;

public class ArcherAI : EnemyAI
{
    public RefactoredProjectile refactoredArcherProjectile;

    // necessary vars for shooting
    public float projectileMaxMoveSpeed;
    public float projectileMaxHeight;

    private Shooter shooterScript;


    void Start() {
        shooterScript = GetComponent<Shooter>();
        Debug.Log("Shooter Script Retrieved");
        shooterScript.InitializeShooter(refactoredArcherProjectile, projectileMaxMoveSpeed, projectileMaxHeight);
        Debug.Log("Shooter Script Initialized");
    }

    // baller
    protected override IEnumerator AttackSubRoutine(float waitTime)
    {
        Debug.Log("Archer attack routine, wait time: " + waitTime);
        while (true)
        {
            if (canAttack)
                Attack();
            yield return new WaitForSeconds(waitTime);
        }
    }


    void Attack()
    {
        // Projectile newProjectile = Instantiate(archerProjectile, transform.position, Quaternion.identity);
        // Debug.Log("Targeting " + attackTarget);
        // newProjectile.damage = enemyData.currentDamage;
        // Vector3 targetPosition = attackTarget.transform.position;
        // FriendlyAI attackTargetAI = attackTarget.GetComponent<FriendlyAI>();
        // if (attackTargetAI != null)
        // {
        //     targetPosition.x += (attackTargetAI.GetMoveSpeed() / Time.fixedDeltaTime) * newProjectile.speed/2;

        //     Debug.Log("Target at: " + attackTarget.transform.position + "\n Target velocity: " + attackTargetAI.GetMoveSpeed() + "\n adjusted position: " + targetPosition);
        // }
        // newProjectile.Launch(targetPosition);
        // Debug.Log("Archer Attack");

        shooterScript.Shoot(attackTarget.transform);

    }
}
