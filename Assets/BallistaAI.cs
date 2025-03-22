using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BallistaAI : EnemyAI
{
    [SerializeField]
    GameObject ballistaHead;
    [SerializeField]
    public GameObject ballistaProjectile;

    // necessary vars for shooting
    [SerializeField]
    public float projectileMaxMoveSpeed;
    [SerializeField]
    public float projectileMaxHeight;
    [SerializeField]
    AudioResource reloadingAudio;

    private Shooter shooterScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        shooterScript = GetComponent<Shooter>();
        shooterScript.InitializeShooter(ballistaProjectile, projectileMaxMoveSpeed, projectileMaxHeight);
    }


    //Only returns Towers
    protected override GameObject GetTarget()
    {
        List<GameObject> potentialTowers = new List<GameObject>();
        // //Debug.Break();
        foreach(GameObject toLookAt in potentialTargets){
            // //Debug.Log("Looking at " + toLookAt);
            // //Debug.Log("Has tag: " + toLookAt.tag);
            if(toLookAt.tag == "Tower"){
                // //Debug.Log("added");
                potentialTowers.Add(toLookAt);
            }

        }
        // //Debug.Break();

        if(potentialTowers.Count <= 0){
            canAttack = false;
            return null;
        }
        return potentialTowers[Random.Range(0, potentialTowers.Count)];
    }

    protected override IEnumerator AttackSubRoutine()
    {
        //Debug.Log("Ballista attack routine, wait time: " + waitTime);
        while (true)
        {
            yield return new WaitForSeconds(enemyData.attackSpeed);
            if (canAttack) {
                // //Debug.Log(potentialTargets.Count);
                try{
                    attackTarget = GetTarget();
                } catch{
                    //Debug.Log("Out of index for some reason");
                }
                
                Attack();
            }
        }
    }


    void Attack()
    {
        // //Debug.Log("Going to attack: " + attackTarget);
        // //Debug.Break();
        if (attackTarget != null) {
            shooterScript.Shoot(attackTarget.transform);
            SoundFXManager.instance.PlaySoundFXClip(reloadingAudio, transform, 1);
        }

    }
    protected override void MoveToTarget(){
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
            transform.localScale = new Vector3(-1, 1, 1);
            movementVector += Vector3.left * enemyData.currentMoveSpeed;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);//flips the enemy around
            movementVector += Vector3.right * enemyData.currentMoveSpeed;
        }
        // //Debug.Log("Moving by " + movementVector);
        transform.position += movementVector;
    }
}
