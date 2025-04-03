
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleStorm : Spell
{
    [SerializeField]
    Transform[] spawnLocations;
    [SerializeField]
    float spawnRandomness = .5f;
    [SerializeField]
    int numIcicles = 10;
    [SerializeField]
    Icicle icicle;
    IEnumerator stormRoutine;
    List<Transform> chosenSpawns = new List<Transform>();
    private static readonly int Done = Animator.StringToHash("Done");


    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }

    public override void CastSpellAt(Vector3 position){
        base.CastSpellAt(position);
        stormRoutine = Storm(duration, numIcicles);
        StartCoroutine(stormRoutine);

    }

    IEnumerator Storm(float stormDuration, int numIcicles){
        float delayTime = stormDuration / numIcicles;
        Debug.Log("" + stormDuration + "");
        Debug.Log("" + numIcicles + "");
        Debug.Log("" + delayTime + "");
        int spawnedIcicles = 0;
        while(spawnedIcicles <= numIcicles){
            SpawnIcicle();
            spawnedIcicles++;
            yield return new WaitForSeconds(delayTime);
        }
        anim.SetTrigger(Done);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    private void SpawnIcicle()
    {
        Vector3 spawnPoint = GetRandomSpawnPoint();
        Icicle newIcicle = Instantiate(icicle, spawnPoint, icicle.transform.rotation);
        newIcicle.SetDamage(damage);
        newIcicle.SetTarget(new Vector3(spawnPoint.x, 0, spawnPoint.z));//Want it to go all the way to the ground
        newIcicle.gameObject.SetActive(true);
    }

    //Returns a random spawn transform (OUT OF THE ONES NOT SELECTED YET)
    //Once all selected allows them to repeat again
    // this lets the spell spread out across the entire transform
    private Transform GetRandomTransform(){
        if(chosenSpawns.Count >= spawnLocations.Length){
            chosenSpawns = new List<Transform>();
        } 
        Transform chosenTransform = spawnLocations[Random.Range(0, spawnLocations.Length)];
        
        while(chosenSpawns.Contains(chosenTransform)){
            chosenTransform = GetRandomTransform();
        }

        return chosenTransform;
    
    }
    private Vector3 GetRandomSpawnPoint()
    {
        Transform transform = GetRandomTransform();
        chosenSpawns.Add(transform);
        Vector3 spawnPoint = transform.position;
        spawnPoint += new Vector3(Random.Range(-spawnRandomness, spawnRandomness),Random.Range(-spawnRandomness, spawnRandomness), 0);
        return spawnPoint;
    }
}
