using System.Collections;
using UnityEngine;

public class Debuff : Spell
{
    [SerializeField]
    CircleCollider2D targetFinder;
    [SerializeField]
    float radius = .1f;
    IEnumerator buffRoutine;

    protected override void Awake()
    {
        base.Awake();
        targetFinder.radius = radius;
    }
    IEnumerator Hold(float duration, GameObject toBuff){
        Debug.Log("" + duration);
        toBuff.GetComponent<EnemyStats>().SendMessage("InitializeValues", damage);
        
        yield return new WaitForSeconds(duration);
        
        toBuff.GetComponent<EnemyStats>().SendMessage("InitializeValues");
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        //Deal the damage
        if(collidedObject.tag == "Enemy"){
            buffRoutine = Hold(damage, collidedObject);
            StartCoroutine(buffRoutine);
        }
    }
}
