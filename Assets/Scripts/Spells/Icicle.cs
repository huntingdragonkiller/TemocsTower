using System;
using UnityEngine;

public class Icicle : MonoBehaviour
{
    [SerializeField]
    Collider2D hitbox;
    [SerializeField]
    Rigidbody2D _rb;
    [SerializeField]
    Animation anim;
    float damage;
    Vector3 targetPosition;

    void FixedUpdate()
    {
        if(!anim.isPlaying){
            hitbox.enabled = true;
        }
        if(transform.position.y < targetPosition.y){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        //Deal the damage
        if (collidedObject.tag == "Enemy"){
            Debug.Log("Attacking " + collidedObject + " for damage: " + damage);
            collidedObject.GetComponent<EnemyStats>().SendMessage("TakeDamage", damage);
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetTarget(Vector3 vector3)
    {
        targetPosition = vector3;
    }
}
