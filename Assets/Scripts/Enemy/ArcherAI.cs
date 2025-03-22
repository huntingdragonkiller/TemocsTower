using System.Collections;
using UnityEngine;

public class ArcherAI : EnemyAI
{
    public GameObject refactoredArcherProjectile;

    // necessary vars for shooting
    public float projectileMaxMoveSpeed;
    public float projectileMaxHeight;

    private Shooter shooterScript;


    protected override void Start() {
        base.Start();
        shooterScript = GetComponent<Shooter>();
        shooterScript.InitializeShooter(refactoredArcherProjectile, projectileMaxMoveSpeed, projectileMaxHeight);
    }

    // baller
    protected override IEnumerator AttackSubRoutine()
    {
        Debug.Log("Archer attack routine, wait time: " + enemyData.attackSpeed);
        while (true)
        {
            yield return new WaitForSeconds(enemyData.attackSpeed);
            if (canAttack) {
                Debug.Log(potentialTargets.Count);
                try{
                    attackTarget = GetTarget();
                } catch{
                    Debug.Log("Out of index for some reason");
                }
                
                Attack();
            }
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

        if (attackTarget != null) {
            shooterScript.Shoot(attackTarget.transform);

        }

    }
}
