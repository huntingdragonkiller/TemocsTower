using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileScriptableObject projectileData;

    //Current stats
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float launchAngle;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public bool isGroundEnemy;
    [HideInInspector]
    public bool tracking;
    public int trackingFrames;


    float actualAngle;
    Rigidbody2D _rb;
    GameObject target;  
    int elapsedFrames = 0;

    void Awake()
    {
        speed = projectileData.Speed;
        launchAngle = projectileData.LaunchAngle;
        actualAngle = launchAngle;
        damage = projectileData.Damage;
        tracking = projectileData.Tracking;
        trackingFrames = projectileData.TrackingFrames;
        _rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag != "Hitbox"){
            collision.collider.SendMessage("TakeDamage", damage);
            Debug.Log("Hit");
            Kill();
        }
    }

    private void FixedUpdate()
    {

        if (target != null && tracking)
        {   
            float interpolationRatio = (float)elapsedFrames / trackingFrames;
            // Debug.Log("My velocity is " + _rb.linearVelocity);
            Vector2 newVelocity = Vector2.Lerp(_rb.linearVelocity.normalized, Track(target), interpolationRatio);
            _rb.linearVelocity = newVelocity * speed;
            
            elapsedFrames = (elapsedFrames + 1) % (trackingFrames + 1);
        }
        Vector2 velocity = _rb.linearVelocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Launch(Vector3 targetPosition)
    {
        if (transform.position.x > targetPosition.x)
            actualAngle = 180 - launchAngle;
        else
            actualAngle = launchAngle;
        float xVelocity = (targetPosition.x - transform.position.x) * 2 / speed;
        float yVelocity = ((targetPosition.y - transform.position.y) / (speed * 0.5f)) - (0.5f * Physics2D.gravity.y * speed * 0.5f);
        _rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        //float requiredVelocity = (new Vector2(xVelocity, yVelocity)).magnitude;
        //_rb.linearVelocity = new Vector2(requiredVelocity * Mathf.Cos(Mathf.Deg2Rad * launchAngle), requiredVelocity * Mathf.Sin(Mathf.Deg2Rad * launchAngle));
    }

    public void TrackingLaunch(GameObject target){
        // float xVelocity = (target.transform.position.x - transform.position.x) / speed;
        // float yVelocity = (target.transform.position.y - transform.position.y) / speed ;
        // _rb.linearVelocity = new Vector2(xVelocity, yVelocity);
        this.target = target;
    }

    Vector2 Track(GameObject target){
        float magnitude = _rb.linearVelocity.magnitude;
        
        Vector2 thisPos = new Vector2(transform.position.x, transform.position.y);
       
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.y);


        //Find the angle for the two Vectors


        
        return new Vector2(targetPos.x - thisPos.x, targetPos.y - thisPos.y).normalized;
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
