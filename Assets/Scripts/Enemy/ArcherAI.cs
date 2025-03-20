using System.Collections;
using UnityEngine;

public class ArcherAI : EnemyAI
{
    public Projectile archerProjectile;

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
        Projectile newProjectile = Instantiate(archerProjectile, transform.position, Quaternion.identity);
        Debug.Log("Targeting " + attackTarget);
        newProjectile.damage = enemyData.currentDamage;
        Vector3 targetPosition = attackTarget.transform.position;
        FriendlyAI attackTargetAI = attackTarget.GetComponent<FriendlyAI>();
        if (attackTargetAI != null)
        {
            targetPosition.x += (attackTargetAI.GetMoveSpeed() / Time.fixedDeltaTime) * newProjectile.speed/2;

            Debug.Log("Target at: " + attackTarget.transform.position + "\n Target velocity: " + attackTargetAI.GetMoveSpeed() + "\n adjusted position: " + targetPosition);
        }
        newProjectile.Launch(targetPosition);
        Debug.Log("Archer Attack");

    }
}
