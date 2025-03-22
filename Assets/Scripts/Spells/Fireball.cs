using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Fireball : Spell
{
    
    [SerializeField] CircleCollider2D explosion;
    [SerializeField]
    Rigidbody2D _rb;
    [SerializeField]
    float initialVelocity = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        // _rb = GetComponent<Rigidbody2D>();
        // explosion = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            Explode();
        }
    }

    public override void CastSpellAt(Vector3 position){
        base.CastSpellAt(position);
        Debug.Log(""+ position + " Compared to"  + transform.position);

        Vector2 directionVector = position - transform.position; 
        directionVector.Normalize();
        _rb.linearVelocity = directionVector * initialVelocity;
        
        Debug.Log("" + directionVector);
        Debug.Log("" + _rb.linearVelocity);

    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, targetPosition) < .1f)
        {
            Explode();
        }
    }

    void Explode() {
        explosion.enabled = true;
        List<Collider2D> contacts = new List<Collider2D>();

        // explosion.Overlap(contacts) fills contacts 
        Debug.Log("Exploding " + explosion.Overlap(contacts) + " GameObjects");
        Debug.Log("explosion collisions: " + contacts);
        foreach(Collider2D collision in contacts){
            GameObject collidedObject = collision.gameObject;
            //Deal the damage
            if (collidedObject.tag == "Enemy"){
                collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", damage);
            }
        }
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
