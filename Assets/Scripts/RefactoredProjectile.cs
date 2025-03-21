using System;
using UnityEngine;

public class RefactoredProjectile : MonoBehaviour
{
    public ProjectileScriptableObject projectileData;

    //Current stats

    [HideInInspector]
    public float damage;
    [HideInInspector]
    public bool isGroundEnemy;

    Rigidbody2D _rb;

    /********************/
    private Transform target;

    [HideInInspector]
    public float projectileSpeed;
    private float moveSpeed;
    private float maxMoveSpeed;
    
    private Vector3 moveDirNormalized;
    private float trajectoryMaxRelativeHeight;

    private AnimationCurve trajectoryAnimationCurve;
    private AnimationCurve axisCorrectionAnimationCurve;
    private AnimationCurve projectileSpeedAnimationCurve;

    private Vector3 trajectoryStartPoint;


    void Awake()
    {
        projectileSpeed = projectileData.Speed;
        damage = projectileData.Damage;
        trajectoryStartPoint = transform.position;
        _rb = GetComponent<Rigidbody2D>();

    }

    public void InitializeProjectile(Transform target, float maxMoveSpeed, float trajectoryMaxHeight) {
        this.target = target;
        this.maxMoveSpeed = maxMoveSpeed;
        
        float xDistanceToTarget = target.position.x - transform.position.x;
        this.trajectoryMaxRelativeHeight = Mathf.Abs(xDistanceToTarget) * trajectoryMaxHeight;
        
    }

    public void InitializeAnimationCurves(AnimationCurve trajectoryAnimationCurve, AnimationCurve axisCorrectionAnimationCurve, AnimationCurve projectileSpeedAnimationCurve) {
        this.trajectoryAnimationCurve = trajectoryAnimationCurve;
        this.axisCorrectionAnimationCurve = axisCorrectionAnimationCurve;
        this.projectileSpeedAnimationCurve = projectileSpeedAnimationCurve;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        if (collision.collider.gameObject.tag == "Ground") {
            KillRefactored();
        }else if(collision.collider.gameObject.tag != "Hitbox" && collision.collider.gameObject.tag != "Projectile"){
            Debug.Log("Collision: " + collision);
            Debug.Log("Collider: " + collision.collider);
            collision.collider.SendMessage("TakeDamage", damage);
            KillRefactored();
        }
        Debug.Log("Collision Done");
    }

    void Update()
    {

        updateProjectilePosition();

    }

    public void updateProjectilePosition() {

        // if target dies while projectile is heading over there
        // using target as a transform allows for tracking
        if (target == null) {
            KillRefactored();
        } else {

            Vector3 trajectoryRange = target.position - trajectoryStartPoint;

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
