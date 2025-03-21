using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretTower : TowerSegment
{
    public float damage;
    public float attackSpeed;
    private IEnumerator attacking;
    bool canAttack;
    List<GameObject> enemies = new List<GameObject>();

    public Collider2D attackFOV;

    public GameObject refactoredTurretProjectile;
    public float projectileMaxMoveSpeed;
    public float projectileMaxHeight;

    private Shooter shooterScript;

    public TurretUIManager turretUIManager;



    public override void Awake()
    {
        base.Awake();
        canAttack = false;
        turretUIManager.UpdateDamage((int)damage);
        turretUIManager.UpdateAttackSpeed(attackSpeed);

    }

    protected override void Start() {
        base.Start();
        attacking = AttackCoroutine(attackSpeed);
        StartCoroutine(attacking);
        shooterScript = GetComponent<Shooter>();
        shooterScript.InitializeShooter(refactoredTurretProjectile, projectileMaxMoveSpeed, projectileMaxHeight);
    }

    public override bool Upgrade()
    {
        if(!base.Upgrade())
            return false;
        
        damage += 5;
        attackSpeed -= 0.1f;
        RestartAttackingCoroutine();
        turretUIManager.UpdateDamage((int)damage);
        turretUIManager.UpdateAttackSpeed(attackSpeed);
        return true;
    }

    void RestartAttackingCoroutine(){
        StopCoroutine(attacking);
        attacking = AttackCoroutine(attackSpeed);
        StartCoroutine(attacking);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //If we something enters the collider, it's in the enemies layer, if it's not the enemies FOV, we can attack it
        if (collision.otherCollider == attackFOV && collision.collider.gameObject.tag != "Hitbox"){
            enemies.Add(collision.collider.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider == attackFOV && collision.collider.gameObject.tag != "Hitbox"){
            enemies.Remove(collision.collider.gameObject);
        }
    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator AttackCoroutine(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            //If there are enemies present if our FOV, attack them
            if (enemies.Count > 0)
            {

                Attack();
            }
        }
    }

    void Attack(){
        // GameObject target = GetTarget();
        // Debug.Log("Targeting " + target.name + " at: " + target.transform.position);
        // RefactoredProjectile newProjectile = Instantiate(refactoredTurretProjectile, transform.position, Quaternion.identity);
        // newProjectile.TrackingLaunch(target);

        // Implement call to Shooter.shoot here
        Debug.Log("1738 AY IM LIKE HEY WASSUP HELLO");
        shooterScript.Shoot(GetTarget().transform);

    }

    //Returns the target dependant on the towers target settings
    //Currently this only returns the enemy that first appeared
    GameObject GetTarget(){
        return enemies[0];
    }
}
