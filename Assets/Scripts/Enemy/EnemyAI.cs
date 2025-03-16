using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    private EnemyStats enemyData;
    public Collider2D attackHitbox;
    LayerMask attackHitboxMask;
    Collider2D hitbox;
    LayerMask hitboxMask;
    public GameObject target;
    bool blocked = false;
    bool canAttack = false;
    GameObject attackTarget;
    List<GameObject> potentialTargets  = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyData = this.GetComponent<EnemyStats>();
        hitbox = GetComponent<Collider2D>();
        hitboxMask = hitbox.callbackLayers;
        attackHitboxMask = attackHitbox.callbackLayers;

    }

    void RetargetTower()
    {
        target = GameObject.FindAnyObjectByType<EnemyManager>().FocusSegment(0); //Default retargets base tower segment
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
            RetargetTower();
        //if(attackHitbox.IsTouchingLayers())
        if(!blocked && target != null)
            MoveToTarget();
        if (canAttack)
            Attack();
        //transform.position.x;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.otherCollider);
        Debug.Log(collision.collider);
        if (collision.otherCollider == attackHitbox)
        {
            //if we are able to attack right now, it means we already have a target
            //therefore we save this new target for later
            if (canAttack)
            {
                potentialTargets.Add(collision.collider.gameObject);
            } else
            {
                //If we cant attack, add this as our current target and update the bool
                attackTarget = collision.collider.gameObject;
                canAttack = true;
            }
            blocked = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //If a collision ended and there are no other collisions, we cant attack so set target to null and update bool
        if (collision.otherCollider == attackHitbox)
        {
            if (!attackHitbox.IsTouchingLayers(attackHitboxMask)) //if the attack hitbox is not colliding with anything it means we cant attack
            {
                Debug.Log("No more enemies");
                if(potentialTargets.Count > 0)
                    potentialTargets.Clear(); //if we can't attack then there shouldn't be potential targets, so we have to clear the list (just in case)
                attackTarget = null;
                blocked = false;
                canAttack = false;
            }
            else if (potentialTargets.Contains(collision.collider.gameObject))
            {
                potentialTargets.Remove(collision.collider.gameObject); //The potential target was killed by something else, so we need to remove it from this list
            } else if((target == null || target == collision.collider.gameObject) && potentialTargets.Count > 0)
            {
                //Update target and remove it from the list of potential targets                
                target = potentialTargets[0];
                potentialTargets.RemoveAt(0);             
            }
        }
    }

    void Attack()
    {
        attackTarget.SendMessage("TakeDamage", enemyData.currentDamage);
    }

    void MoveToTarget()
    {
        Vector3 movementVector = Vector3.zero;

        if (target.transform.position.y > transform.position.y && !enemyData.isGroundEnemy)
        {
            movementVector += Vector3.up * enemyData.currentMoveSpeed;
        } else if (target.transform.position.y < transform.position.y && !enemyData.isGroundEnemy)
        {
            movementVector += Vector3.down * enemyData.currentMoveSpeed;
        }

        //transform.position = new Vector3(Vector3.Lerp(transform.position, target.transform.position, enemyData.currentMoveSpeed).x, transform.position.y, 0);
        if (transform.position.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            movementVector += Vector3.left * enemyData.currentMoveSpeed;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);//flips the enemy around
            movementVector += Vector3.right * enemyData.currentMoveSpeed;
        }
        transform.position += movementVector;
    }
}
