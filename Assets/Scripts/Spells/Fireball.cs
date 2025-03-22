using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    public SpellScriptableObject spellData;
    private float damage;
    private float cooldownTime;
    private int manaCost;
    
    [SerializeField] CircleCollider2D explosion;
    [SerializeField] float explosionDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        explosion = GetComponent<CircleCollider2D>();
        damage = spellData.damage;
        cooldownTime = spellData.cooldownTime;
        manaCost = spellData.manaCost;
        explosionDamage = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Explode();
        }
    }

    void Explode() {
        explosion.enabled = true;
        List<Collider2D> contacts = new List<Collider2D>();

        // explosion.Overlap(contacts) fills contacts 
        Debug.Log("Exploding " + explosion.Overlap(contacts) + " GameObjects");
        Debug.Log("explosion collisions: " + contacts);
        Destroy(gameObject);

    }

    public void OnDestroy()
    {
        
        
        // foreach(Collider2D collision in contacts){
        //     GameObject collidedObject = collision.gameObject;
        //     //Deal the damage
        //     if(collidedObject.tag == "Tower"){
        //         collidedObject.GetComponent<TowerSegment>().SendMessage("TakeDamage", friendlyDamage);
        //     }else if(collidedObject.tag == "Friendly"){
        //         collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", friendlyDamage);
        //     } else if (collidedObject.tag == "Enemy"){
        //         collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", baseDamage);
        //     }
        // }
        // Destroy(gameObject);
    }

}
