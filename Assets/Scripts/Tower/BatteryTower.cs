using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BatteryTower : TowerSegment
{
    [SerializeField]
    float baseDamage = 100f;
    [SerializeField]
    float manaStored = 50f;
    float friendlyDamage;
    BatteryUIManager batteryUIManager;
    private IEnumerator selfHeal;
    public Collider2D explosion;
    public override void Awake()
    {
        base.Awake();
        explosion.enabled = false;
        batteryUIManager = GetComponentInChildren<BatteryUIManager>();
        friendlyDamage = baseDamage;
        batteryUIManager.UpdateStorage(manaStored);
        batteryUIManager.UpdateFriendlyDamage(friendlyDamage, baseDamage);
        selfHeal = SelfHeal();
        StartCoroutine(selfHeal);
    }

    //Self heals by 1 percent max health per second
    private IEnumerator SelfHeal()
    {
        while(true){
            // Disables coroutine while there isn't a wave
             while(!FindAnyObjectByType<EnemyManager>().GetActiveWave()){
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("Healing for: " + maxHealth / 100f);
            HealDamage(maxHealth/100f);
            yield return new WaitForSeconds(1f * localTimeScale);
        }
    }

    public override bool Upgrade()
    {
        if(!base.Upgrade())
            return false;
        
        maxHealth += 50;
        friendlyDamage -= baseDamage * 0.1f;
        manaStored += 50;
        batteryUIManager.UpdateHealth(currentHealth, maxHealth);
        batteryUIManager.UpdateStorage(manaStored);
        batteryUIManager.UpdateFriendlyDamage(friendlyDamage, baseDamage);
        // RestartHealRoutine();
        return true;
    }

    private void RestartHealRoutine()
    {
        StopCoroutine(selfHeal);
        selfHeal = SelfHeal();
        StartCoroutine(selfHeal);


    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     GameObject collidedObject = collision.collider.gameObject;
    //     //Deal the damage
    //     if(collidedObject.tag == "Tower"){
    //         collidedObject.GetComponent<TowerSegment>().SendMessage("TakeDamage", friendlyDamage);
    //     }else if(collidedObject.tag == "Friendly"){
    //         collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", friendlyDamage);
    //     } else {
    //         collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", baseDamage);
    //     }
    // }
    public override void Kill()
    {
        GetComponent<BoxCollider2D>().enabled = false;//disable our damage hitbox, just incase the explosion collides w/ itself
        explosion.enabled = true;
        List<Collider2D> contacts = new List<Collider2D>();
        Debug.Log("Exploding " + explosion.Overlap(contacts) + " GameObjects");
        
        foreach(Collider2D collision in contacts){
            GameObject collidedObject = collision.gameObject;
            //Deal the damage
            if(collidedObject.tag == "Tower"){
                collidedObject.GetComponent<TowerSegment>().SendMessage("TakeDamage", friendlyDamage);
            }else if(collidedObject.tag == "Friendly"){
                collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", friendlyDamage);
            } else if (collidedObject.tag == "Enemy"){
                collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", baseDamage);
            }
        }
        base.Kill();
    }
}
