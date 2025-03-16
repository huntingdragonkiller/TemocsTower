using System.Collections;
using UnityEngine;

public class ArcherAI : EnemyAI
{
    public Projectile archerProjectile;

    // baller
    protected override IEnumerator AttackSubRoutine(float waitTime)
    {
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
        newProjectile.Launch(attackTarget.transform.position);
        Debug.Log("Archer Attack");

    }
}
