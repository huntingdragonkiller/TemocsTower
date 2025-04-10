using System.Collections;
using UnityEngine;

public class Barrier : Spell
{
    [SerializeField]
    Collider2D barrier;
    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(Hold(duration));  
        if (transform.position.x > 0)
        {
            transform.localScale.Set(-1, 1, 1);
        }
    }

    
    void FixedUpdate()
    {
        if (transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    IEnumerator Hold(float duration){
        Debug.Log("" + duration);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.gameObject;
        if (collidedObject.tag == "Enemy"){
            
            Debug.Log("knocking back " + collidedObject + " by: " + damage);
            collidedObject.GetComponent<EnemyAI>().SendMessage("TakeKnockback", damage);
        } else if(collidedObject.tag == "Hitbox"){
            //destroy enemy projectile, definitely a better way to do this but we cuttin corners rn
            if (collidedObject.name.Contains("ectile"))
            {
                Destroy(collidedObject);
            }
        }
    }
}
