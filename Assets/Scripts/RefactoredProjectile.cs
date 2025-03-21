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

    /********************/
    private Transform targetInitialPosition;

    private float moveSpeed;
    private float maxMoveSpeed;
    private Vector3 moveDirNormalized;
    private float trajectoryMaxRelativeHeight;

    private float distanceToTargetToDestroyProjectile = 1f;

    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrectionAnimationCurve;
    private AnimationCurve projectileSpeedAnimationCurve;

    private Vector3 trajectoryStartPoint;


    void Awake()
    {
        speed = projectileData.Speed;
        damage = projectileData.Damage;
        trajectoryStartPoint = transform.position;
        _rb = GetComponent<Rigidbody2D>();

    }

    public void InitializeProjectile(Transform target, float maxMoveSpeed, float trajectoryMaxHeight) {
        this.targetInitialPosition = target;
        this.maxMoveSpeed = maxMoveSpeed;
        
        float xDistanceToTarget = targetInitialPosition.position.x - transform.position.x;
        this.trajectoryMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * trajectoryMaxHeight;
        
        Debug.Log("Projectile Target: " + targetInitialPosition);
    }

    public void InitializeAnimationCurves(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve, AnimationCurve projectileSpeedAnimationCurve) {
        this.trajectoryAnimationCurve = trajectoryAnimationCurve;
        this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve;
        this.projectileSpeedAnimationCurve = projectileSpeedAnimationCurve;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag != "Hitbox"){
            collision.collider.SendMessage("TakeDamage", damage);
            Debug.Log("Collision: " + collision);
            Debug.Log("Collider: " + collision.collider);
            // KillRefactored();
        }
    }

    // private void FixedUpdate()
    // {

    //     // if (target != null && tracking)
    //     // {   
    //     //     float interpolationRatio = (float)elapsedFrames / trackingFrames;
    //     //     // Debug.Log("My velocity is " + _rb.linearVelocity);
    //     //     Vector2 newVelocity = Vector2.Lerp(_rb.linearVelocity.normalized, Track(target), interpolationRatio);
    //     //     _rb.linearVelocity = newVelocity * speed;
            
    //     //     elapsedFrames = (elapsedFrames + 1) % (trackingFrames + 1);
    //     // }
    //     // Vector2 velocity = _rb.linearVelocity;
    //     // float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
    //     // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    // }

    void Update()
    {

        updateProjectilePosition();

    }

    public void updateProjectilePosition() {
        Vector3 trajectoryRange = targetInitialPosition.position - trajectoryStartPoint;

        // If shooter is behind target
        if (trajectoryRange.x < 0) {
            moveSpeed = -moveSpeed;
        }

        float nextPositionX = transform.position.x + moveSpeed * Time.deltaTime;
        float nextPositionXNormalized = (nextPositionX - trajectoryStartPoint.x) / trajectoryRange.x;


        float nextPositionYNormalized = trajectoryAnimationCurve.Evaluate(nextPositionXNormalized);
        float nextPositionYCorrectionNormalized = axisCorrectionAnimationCurve.Evaluate(nextPositionXNormalized);
        float nextPositionYCorrectionAbsolute = nextPositionYCorrectionNormalized * trajectoryRange.y;
        
        float nextPositionY = trajectoryStartPoint.y + nextPositionYNormalized * trajectoryMaxRelativeHeight + nextPositionYCorrectionAbsolute;

        Vector3 newPosition = new Vector3(nextPositionX, nextPositionY, 0);
        
        calculateProjectileSpeed(nextPositionXNormalized);
        
        transform.position = newPosition;
    }

    public void calculateProjectileSpeed(float nextPositionXNormalized) {
        float nextMoveSpeedNormalized = projectileSpeedAnimationCurve.Evaluate(nextPositionXNormalized);

        moveSpeed = nextMoveSpeedNormalized * maxMoveSpeed;
    }

    public void KillRefactored()
    {
        Destroy(gameObject);
    }
}
