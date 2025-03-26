using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debuff : Spell
{
    [SerializeField]
    CircleCollider2D targetFinder;
    [SerializeField]
    float radius = .1f;
    IEnumerator buffRoutine;
    List<GameObject> debuffedEnemies = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        transform.localScale = transform.localScale * radius;
    }

    void Start()
    {
        StartCoroutine(Hold());
    }

    IEnumerator Hold()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    IEnumerator DebuffTarget(GameObject toBuff){
        Debug.Log("" + duration);
        toBuff.GetComponent<EnemyStats>().SendMessage("InitializeValues", damage);
        
        yield return new WaitForSeconds(duration);
        
        toBuff.GetComponent<EnemyStats>().SendMessage("InitializeValues", 1f);
        //Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        //Deal the damage
        if(collidedObject.tag == "Enemy"){

            collidedObject.SendMessage("InitializeValues", damage);
            debuffedEnemies.Add(collidedObject);//Want to track what enemies have entered the circle to handle when the circle is destroyed
            //StartCoroutine(DebuffTarget(collidedObject));
            //StartCoroutine(buffRoutine);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        //Deal the damage
        if (collidedObject.tag == "Enemy")
        {

            collidedObject.SendMessage("InitializeValues", 1f);
            debuffedEnemies.Remove(collidedObject);//Want to track what enemies have entered the circle to handle when the circle is destroyed
            
            //contacts.Add(collision.collider);//Want to track what enemies have entered the circle to handle when the circle is destroyed
            //StartCoroutine(DebuffTarget(collidedObject));
            //StartCoroutine(buffRoutine);
        }
    }

    //Resetting the stats for enemies that were still debuffed and inside the circle when the duration runs out
    //Not sure if this will deal with the enemies that leave the radius before the duration ends
    private void OnDestroy()
    {
        foreach (GameObject enemy in debuffedEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<EnemyStats>().SendMessage("InitializeValues", 1f);
            }
        }


        //List<Collider2D> contacts = new List<Collider2D>();

        //transform.localScale = transform.localScale * 3;
        //// explosion.Overlap(contacts) fills contacts 
        //Debug.Log("Debuffing " + targetFinder.Overlap(contacts) + " GameObjects");
        //Debug.Log("debuff collisions: " + contacts);
        //foreach (Collider2D collision in contacts)
        //{
        //    GameObject collidedObject = collision.gameObject;
        //    //Deal the damage
        //    if (collidedObject.tag == "Enemy")
        //    {
        //        collidedObject.GetComponent<EnemyStats>().SendMessage("InitializeValues", 1f);
        //    }
        //}
    }
}
