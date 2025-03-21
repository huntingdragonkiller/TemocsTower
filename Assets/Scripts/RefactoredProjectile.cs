using System;
using UnityEngine;

public class RefactoredProjectile : MonoBehaviour
{
    public ProjectileScriptableObject projectileData;

    //Current stats
    [HideInInspector]
    public float speed;

    [HideInInspector]
    public float damage;
    [HideInInspector]
    public bool isGroundEnemy;

    Rigidbody2D _rb;
    GameObject target;  

    
    /*
    Things to get rid of: 
        Vars:
            speed (?)
            launchAngle
            tracking
            trackingFrames
            actualAngle
            elapsedFrames

        Methods 
            Launch
            TrackingLaunch
            Track
    */

    void Awake()
    {
        speed = projectileData.Speed;
        damage = projectileData.Damage;
        _rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag != "Hitbox"){
            collision.collider.SendMessage("TakeDamage", damage);
            Debug.Log("Hit");
            KillRefactored();
        }
    }

    private void FixedUpdate()
    {

        // if (target != null && tracking)
        // {   
        //     float interpolationRatio = (float)elapsedFrames / trackingFrames;
        //     // Debug.Log("My velocity is " + _rb.linearVelocity);
        //     Vector2 newVelocity = Vector2.Lerp(_rb.linearVelocity.normalized, Track(target), interpolationRatio);
        //     _rb.linearVelocity = newVelocity * speed;
            
        //     elapsedFrames = (elapsedFrames + 1) % (trackingFrames + 1);
        // }
        // Vector2 velocity = _rb.linearVelocity;
        // float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    public void KillRefactored()
    {
        Destroy(gameObject);
    }
}
