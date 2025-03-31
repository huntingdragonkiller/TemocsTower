using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class EnemyAI : MonoBehaviour
{
    protected EnemyStats enemyData;
    public Collider2D attackHitbox;
    LayerMask attackHitboxMask;
    Collider2D hitbox;
    LayerMask hitboxMask;
    public GameObject target;
    bool blocked = false;
    public bool canAttack = false;
    public GameObject attackTarget;
    protected List<GameObject> potentialTargets  = new List<GameObject>();
    private IEnumerator attackCoroutine;
    
    protected static readonly int IsMoving = Animator.StringToHash("IsMoving");
    
    protected static readonly int AttackTrigger = Animator.StringToHash("Attack");

    protected Animator anim;

    
    void Awake()
    {
        enemyData = this.GetComponent<EnemyStats>();
        hitbox = GetComponent<Collider2D>();
        hitboxMask = hitbox.callbackLayers;
        attackHitboxMask = attackHitbox.callbackLayers;
        canAttack = false;
        anim = GetComponent<Animator>();
        
        // Debug.Log("Enemy Awake");
    }

    protected virtual void Start() {
        // Debug.Log("Enemy Start");
        canAttack = false;
        attackCoroutine = AttackSubRoutine();
        StartCoroutine(attackCoroutine);
    }

    void OnDestroy()
    {
        StopAllCoroutines();        
    }

    public int GetSpawnPoints(){
        return enemyData.spawnCost;
    }

    public bool IsGroundEnemy(){
        return enemyData.isGroundEnemy;
    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator AttackSubRoutine()
    {
        Debug.Log("In attack routine");
        while (true)
        {
            yield return new WaitForSeconds(enemyData.attackSpeed);
            if (canAttack){
                attackTarget = GetTarget();
                anim.SetTrigger(AttackTrigger);
                // Attack();
            }
        }
    }

    void Attack()
    {
        
        Debug.Log("Attack? " + attackTarget);
        if(attackTarget != null){
            Debug.Log("Attacking: " + attackTarget);
            attackTarget.SendMessage("TakeDamage", enemyData.currentDamage);
            SoundFXManager.instance.PlaySoundFXClip(enemyData.attackSoundClip, transform, 1f);
        }
        anim.ResetTrigger(AttackTrigger);
    }

    protected virtual GameObject GetTarget()
    {
        return potentialTargets[0];
    }

    protected void RetargetTower()
    {
        target = GameObject.FindAnyObjectByType<EnemyManager>().FocusSegment(0); //Default retargets base tower segment
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(potentialTargets.Count);
        if(potentialTargets.Count > 0){
            // Debug.Log("AttackTime");
            canAttack = true;
            anim.SetBool(IsMoving, false);
        } else if(attackTarget == null){
            // Debug.Log("Can't attack, going to move");
            canAttack = false;
            anim.SetBool(IsMoving, true);
            MoveToTarget();
        }
            
        
        //if(attackHitbox.IsTouchingLayers())
        
        //transform.position.x;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(collision.otherCollider);
        // Debug.Log(collision.collider.gameObject.tag);
        if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox"){
            
            Debug.Log(collision.collider.gameObject);
            // Debug.Break();
            potentialTargets.Add(collision.collider.gameObject);
            canAttack = true;
        }
        // // Debug.Log(collision.otherCollider);
        // // Debug.Log(collision.collider);
        // if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox")
        // {
        //     potentialTargets.Add(collision.collider.gameObject);
        //     //if we are able to attack right now, it means we already have a target
        //     //therefore we save this new target for later
        //     if (!canAttack)
        //     {
        //         //If we cant attack, add this as our current target and update the bool
        //         attackTarget = collision.collider.gameObject;
        //         potentialTargets.RemoveAt(0); //since we added it as a potential target, we need to remove it now.
        //                                       //We implement it like this to handle collisions that occur simultaneously.
        //         canAttack = true;
        //     }
        //     blocked = true;

        // } else if(collision.otherCollider == hitbox && collision.collider.gameObject.tag != "Hitbox")
        // {
        //     blocked = true;
        // }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox"){
            potentialTargets.Remove(collision.collider.gameObject);
        }
        // //If a collision ended and there are no other collisions, we cant attack so set target to null and update bool
        // if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox")
        // {
        //     if (!attackHitbox.IsTouchingLayers(attackHitboxMask)) //if the attack hitbox is not colliding with anything it means we cant attack
        //     {
        //         Debug.Log("No more enemies");
        //         if(potentialTargets.Count > 0)
        //             potentialTargets.Clear(); //if we can't attack then there shouldn't be potential targets, so we have to clear the list (just in case)
        //         attackTarget = null;
        //         blocked = false;
        //         canAttack = false;
        //     }
        //     else if (potentialTargets.Contains(collision.collider.gameObject))
        //     {
        //         potentialTargets.Remove(collision.collider.gameObject); //The potential target was killed by something else, so we need to remove it from this list
        //     } else if((target == null || target == collision.collider.gameObject) && potentialTargets.Count > 0)
        //     {
        //         Debug.Log("Selecting new target");
        //         //Update target and remove it from the list of potential targets                
        //         target = potentialTargets[0];
        //         attackTarget = target;
        //         potentialTargets.RemoveAt(0);             
        //     }
        //     else if (collision.otherCollider == hitbox && collision.collider.gameObject.tag != "Hitbox")
        //     {
        //         blocked = false;
        //     }
        // }
    }


    protected virtual void MoveToTarget()
    {

        if(target == null)
        {
            RetargetTower();
        }

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
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            movementVector += Vector3.left * enemyData.currentMoveSpeed;
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z); //flips the enemy around
            movementVector += Vector3.right * enemyData.currentMoveSpeed;
        }
        // Debug.Log("Moving by " + movementVector);
        transform.position += movementVector;
    }
    
    public void TakeKnockback(float knockback)
    {
        //TODO: Make the target go backwards from current facing direction in an arc
        // thx emilio although this might work for now
        float xScale = transform.localScale.x / Mathf.Abs(transform.localScale.x);//should be -1 if on left side of tower, 1 on right side of tower
        //ABOVE DOES NOT WORK FOR BALLISTAS CURRENTLY LOL!!!!
        transform.position = transform.position + new Vector3(xScale * knockback, 0, 0);
    }
}
