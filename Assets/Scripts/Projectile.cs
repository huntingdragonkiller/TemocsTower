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

    float actualAngle;
    Rigidbody2D _rb;

    void Awake()
    {
        speed = projectileData.Speed;
        launchAngle = projectileData.LaunchAngle;
        actualAngle = launchAngle;
        damage = projectileData.Damage;
        _rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.SendMessage("TakeDamage", damage);
        Debug.Log("Hit");
        Kill();
    }

    private void FixedUpdate()
    {
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

    public void Kill()
    {
        Destroy(gameObject);
    }
}
