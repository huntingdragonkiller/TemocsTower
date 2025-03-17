using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FriendlyAI : MonoBehaviour
{
    protected EnemyStats friendlyData;
    public Collider2D attackHitbox;
    LayerMask attackHitboxMask;
    Collider2D hitbox;
    LayerMask hitboxMask;
    public GameObject target;
    bool blocked = false;
    public bool canAttack = false;
    public GameObject attackTarget;
    List<GameObject> potentialTargets = new List<GameObject>();
    private IEnumerator attackCoroutine;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        friendlyData = this.GetComponent<EnemyStats>();
        hitbox = GetComponent<Collider2D>();
        hitboxMask = hitbox.callbackLayers;
        attackHitboxMask = attackHitbox.callbackLayers;
        attackCoroutine = AttackSubRoutine(friendlyData.attackSpeed);
        StartCoroutine(attackCoroutine);

    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator AttackSubRoutine(float waitTime)
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
        attackTarget.SendMessage("TakeDamage", friendlyData.currentDamage);
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
        if (!blocked && target != null)
            Move();
        //transform.position.x;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log(collision.otherCollider);
        Debug.Log(collision.collider);
        if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox")
        {
            potentialTargets.Add(collision.collider.gameObject);
            //if we are able to attack right now, it means we already have a target
            //therefore we save this new target for later
            if (!canAttack)
            {
                //If we cant attack, add this as our current target and update the bool
                attackTarget = collision.collider.gameObject;

                potentialTargets.RemoveAt(0); //since we added it as a potential target, we need to remove it now.
                                              //We implement it like this to handle collisions that occur simultaneously.
                canAttack = true;
            }
            blocked = true;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //If a collision ended and there are no other collisions, we cant attack so set target to null and update bool
        if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox")
        {
            if (!attackHitbox.IsTouchingLayers(attackHitboxMask)) //if the attack hitbox is not colliding with anything it means we cant attack
            {
                Debug.Log("No more enemies");
                if (potentialTargets.Count > 0)
                    potentialTargets.Clear(); //if we can't attack then there shouldn't be potential targets, so we have to clear the list (just in case)
                attackTarget = null;
                blocked = false;
                canAttack = false;
            }
            else if (potentialTargets.Contains(collision.collider.gameObject))
            {
                potentialTargets.Remove(collision.collider.gameObject); //The potential target was killed by something else, so we need to remove it from this list
            }
            else if ((attackTarget == null || attackTarget == collision.collider.gameObject) && potentialTargets.Count > 0)
            {
                Debug.Log("Selecting new target");
                //Update target and remove it from the list of potential targets                
                target = potentialTargets[0];
                attackTarget = target;
                potentialTargets.RemoveAt(0);
            }
        }
    }


    void Move()
    {
        Vector3 movementVector = Vector3.zero;

        if (target.transform.position.y > transform.position.y && !friendlyData.isGroundEnemy)
        {
            movementVector += Vector3.up * friendlyData.currentMoveSpeed;
        }
        else if (target.transform.position.y < transform.position.y && !friendlyData.isGroundEnemy)
        {
            movementVector += Vector3.down * friendlyData.currentMoveSpeed;
        }

        //transform.position = new Vector3(Vector3.Lerp(transform.position, target.transform.position, friendlyData.currentMoveSpeed).x, transform.position.y, 0);
        if (transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            movementVector += Vector3.left * friendlyData.currentMoveSpeed;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);//flips the friendly around
            movementVector += Vector3.right * friendlyData.currentMoveSpeed;
        }
        transform.position += movementVector;
    }

    public float GetMoveSpeed()
    {
        if (blocked)
            return 0;
        if (transform.position.x < 0)
        {
            return -1 * friendlyData.currentMoveSpeed;
        } else
        {

            return 1 * friendlyData.currentMoveSpeed;
        }
    }
}


