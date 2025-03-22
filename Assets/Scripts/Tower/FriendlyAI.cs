using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class FriendlyAI : MonoBehaviour
{
    protected EnemyStats friendlyData;
    public Collider2D attackHitbox;
    public float fallSpeedTilePerSecond = 2f;
    LayerMask attackHitboxMask;
    Collider2D hitbox;
    LayerMask hitboxMask;
    public EnemyAI target;
    bool blocked = false;
    public bool canAttack = false;
    List<EnemyAI> enemies = new List<EnemyAI>();
    private IEnumerator attackCoroutine;
    bool inAir = true;
    float fallSpeed;
    public float patrolRadiusFromOrigin = 5f;
    public float detectEnemiesDistance = 10f;
    private bool _movingRight;
    private bool enemiesPresent = false;
    float currentMoveSpeed;
    float moveSpeedVariation = 0.05f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Debug.Log("My height is: " + transform.position.y);
        // Debug.Log("My local height is: " + transform.localPosition.y);
        fallSpeed = fallSpeedTilePerSecond * Time.fixedDeltaTime;
        friendlyData = this.GetComponent<EnemyStats>();
        hitbox = GetComponent<Collider2D>();
        hitboxMask = hitbox.includeLayers;
        attackHitboxMask = attackHitbox.includeLayers;
       
    }
    void Start()
    { 
        currentMoveSpeed = friendlyData.currentMoveSpeed * (1f + UnityEngine.Random.Range(-moveSpeedVariation, moveSpeedVariation));

        attackCoroutine = AttackSubRoutine(friendlyData.attackSpeed);
        StartCoroutine(attackCoroutine);
    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator AttackSubRoutine(float waitTime)
    {
        Debug.Log("Starting: " + waitTime);
        while (true)
        {
            if (enemies.Count > 0)
            {
                Attack();
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    void Attack()
    {
        EnemyAI target = GetTarget();
        if(target != null){
            target.SendMessage("TakeDamage", friendlyData.currentDamage);
            SoundFXManager.instance.PlaySoundFXClip(friendlyData.attackSoundClip, transform, 1f);
        }
        Debug.Log("Attacking: " + target);
    }

    //Returns the target dependant on the towers target settings
    //Currently this only returns the enemy that first appeared
    EnemyAI GetTarget(){
        return enemies[0];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y <= 0)
        {
            inAir = false;
            transform.position = new Vector3(transform.position.x, 0, transform.position.z); //snap to y = 0
        }
        //if(attackHitbox.IsTouchingLayers())
        if (enemies.Count <= 0)
            Move();
        enemiesPresent = CheckForEnemies();
        //transform.position.x;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //If we something enters the collider, it's in the enemies layer, if it's not the enemies FOV, we can attack it
        if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox"){
            enemies.Add(collision.collider.gameObject.GetComponent<EnemyAI>());
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //if its left the collider we cant attack it
        if (collision.otherCollider == attackHitbox && collision.collider.gameObject.tag != "Hitbox"){
            enemies.Remove(collision.collider.gameObject.GetComponent<EnemyAI>());
            if(enemies.Count <= 0){
                //adds a small chance to randomly change directions, just to spice up movement
                if(UnityEngine.Random.Range(0,3) == 0)
                {
                    _movingRight = !_movingRight;
                }
                enemiesPresent = CheckForEnemies();
            }
        }
    }

    void Move()
    {
        Vector3 movementVector = Vector3.zero;
        //if in the air fall down until we hit the ground then do normal movement
        if(inAir){
            movementVector = Vector3.down * fallSpeed;
            transform.position += movementVector;
            return;
        } else if(!enemiesPresent){
            movementVector = Patrol();
        } else {
             movementVector = MoveToEnemy();
        }

        transform.position += movementVector;
    }

    //If there are enemies move in the direction we last spotted them at
    private Vector3 MoveToEnemy()
    {
        Vector3 movementVector = Vector3.right * currentMoveSpeed;
        float moveDirection = _movingRight ? 1f : -1f;
        // enemiesPresent = CheckForEnemies();
        return movementVector * moveDirection;
    }

    Vector3 Patrol()
    {
        Vector3 movementVector = Vector3.right * currentMoveSpeed;
        float moveDirection = _movingRight ? 1f : -1f;

        // Check if the enemy reached a patrol point
        if (_movingRight && transform.position.x >= patrolRadiusFromOrigin)
        {
            _movingRight = false;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (!_movingRight && transform.position.x <= -patrolRadiusFromOrigin)
        {
            _movingRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        // enemiesPresent = CheckForEnemies();
        
        return movementVector * moveDirection;
        
    }

    bool CheckForEnemies(){
        Vector3 direction = _movingRight ? Vector3.right : Vector3.left;
        
        
        // Debug.DrawLine(transform.position, direction * detectEnemiesDistance, Color.red, 0.2f);
        // Debug.Log(attackHitboxMask.value);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectEnemiesDistance, attackHitboxMask);
        if (hit)
        {
            // Debug.Log("Enemy Detected! " + hit.collider.gameObject.name + "at: " + hit.collider.gameObject.transform.position);
            enemiesPresent = true;
            return true;
        }
        hit = Physics2D.Raycast(transform.position, direction * -1, detectEnemiesDistance, attackHitboxMask);
        if (hit)
        {
            if(!enemiesPresent){
                _movingRight = !_movingRight;
            } else if (UnityEngine.Random.Range(0,2) == 0){
                // 50/50 to pick a direction
                _movingRight = !_movingRight;
            }
            // Debug.Log("Enemy Detected! " + hit.collider.gameObject.name + "at: " + hit.collider.gameObject.transform.position);
            enemiesPresent = true;
            return true;
        }
        return false;
    }

    public float GetMoveSpeed()
    {
        if (enemies.Count > 0)
            return 0;
        if (!_movingRight)
        {
            return -1 * currentMoveSpeed;
        } else
        {

            return 1 * currentMoveSpeed;
        }
    }
}


