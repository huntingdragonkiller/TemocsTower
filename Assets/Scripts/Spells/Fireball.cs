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

    private static readonly int Done = Animator.StringToHash("Done");
    private Animator anim;

    IEnumerator exploding;
    Vector2 targetVelocity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
        // _rb = GetComponent<Rigidbody2D>();
        // explosion = GetComponent<CircleCollider2D>();
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha0)) {
        //     Explode();
        // }
    }

    public override void CastSpellAt(Vector3 position){
        base.CastSpellAt(position);
        Debug.Log(""+ position + " Compared to"  + transform.position);

        Vector2 directionVector = position - transform.position; 
        directionVector.Normalize();
        targetVelocity = directionVector * initialVelocity;
        
        Debug.Log("" + directionVector);
        Debug.Log("" + targetVelocity);
        Debug.Log("" + _rb.linearVelocity);

    }

    public void CastDone()
    {
        _rb.linearVelocity = targetVelocity;
    }

    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, targetPosition) < .1f)
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode() {
        _rb.linearVelocity = Vector2.zero;
        anim.SetTrigger(Done);
        explosion.enabled = true;
        List<Collider2D> contacts = new List<Collider2D>();
        //transform.localScale = transform.localScale * 3 ;
        explosion.radius *= 2;
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
        yield return null;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Ground"){
            if (exploding == null)
            {
                exploding = Explode();
                StartCoroutine(exploding);

            }
        }
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
