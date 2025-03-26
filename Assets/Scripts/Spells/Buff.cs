using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : Spell
{
    [SerializeField]
    CircleCollider2D targetFinder;
    [SerializeField]
    float radius = .1f;
    IEnumerator buffRoutine;
    List<GameObject> buffedTowers = new List<GameObject>();

    protected override void Awake()
    {
        base.Awake();
        targetFinder.radius = radius;
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

    IEnumerator BuffTarget(GameObject toBuff){
        Debug.Log("" + duration);
        toBuff.GetComponent<TowerSegment>().SendMessage("SetLocalTimeScale", damage);
        yield return new WaitForSeconds(duration);
        
        toBuff.GetComponent<TowerSegment>().SendMessage("SetLocalTimeScale", 1f);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        //Add Buff
        if(collidedObject.tag == "Tower"){
            StartCoroutine(BuffTarget(collidedObject));
            buffedTowers.Add(collidedObject);//Want to track what enemies have entered the circle to handle when the circle is destroyed
            //StartCoroutine(buffRoutine);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        GameObject collidedObject = collision.collider.gameObject;
        //Remove Buff
        if (collidedObject.tag == "Tower")
        {
            collidedObject.GetComponent<TowerSegment>().SendMessage("SetLocalTimeScale", 1f);
            buffedTowers.Remove(collidedObject);//Want to track what enemies have entered the circle to handle when the circle is destroyed
            //StartCoroutine(buffRoutine);
        }
    }

    //Resetting the stats for towers that were still buffed and inside the circle when the duration runs out
    private void OnDestroy()
    {
        foreach (GameObject tower in buffedTowers)
        {
            if (tower != null)
            {
                tower.GetComponent<TowerSegment>().SendMessage("SetLocalTimeScale", 1f);
            }
        }
    }
}
