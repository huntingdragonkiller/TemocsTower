using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretTower : TowerSegment
{
    public float damage;
    public float attackSpeed;
    private IEnumerator generateMana;
    bool canAttack;
    List<GameObject> enemies = new List<GameObject>();
    public Collider2D attackFOV;
    public Projectile turretProjectile;

    private void Awake()
    {
        canAttack = false;
        generateMana = GenerateMana(attackSpeed);
        StartCoroutine(generateMana);

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
    protected virtual IEnumerator GenerateMana(float waitTime)
    {
        while (true)
        {
            //If there are enemies present if our FOV, attack them
            if (enemies.Count > 0)
            {
                Attack();
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    void Attack(){
        GameObject target = GetTarget();
        Debug.Log("Targeting " + target.name + " at: " + target.transform.position);
        Projectile newProjectile = Instantiate(turretProjectile, transform.position, Quaternion.identity);
        newProjectile.TrackingLaunch(target);
    }

    //Returns the target dependant on the towers target settings
    //Currently this only returns the enemy that first appeared
    GameObject GetTarget(){
        return enemies[0];
    }
}
