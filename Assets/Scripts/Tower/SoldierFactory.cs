using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SoldierFactory : TowerSegment
{
    [SerializeField]
    AudioResource creationSound;
    public GameObject[] spawnLocations;
    public FactoryUIManager factoryUIManager;

    public FriendlyAI soldier;
    public float generateSpeed;
    private IEnumerator createSoldier;
    List<FriendlyAI> friends = new List<FriendlyAI>();
    [SerializeField]
    private int maxSoldiers;
    SummoningCircle summoningCircle;

    public override void Awake()
    {
        base.Awake();
        summoningCircle = GetComponentInChildren<SummoningCircle>();
        createSoldier = CreateSoldier();
        StartCoroutine(createSoldier);
        factoryUIManager.UpdateMaxSoldiers(maxSoldiers);
        factoryUIManager.UpdateProdSpeed(generateSpeed);


    }

    public override bool Upgrade()
    {
        if(!base.Upgrade())
            return false;
        
        maxSoldiers++;
        factoryUIManager.UpdateMaxSoldiers(maxSoldiers);
        return true;
    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator CreateSoldier()
    {
        while (true)
        {
            // Disables coroutine while there isn't a wave
            //  while(!FindAnyObjectByType<EnemyManager>().GetActiveWave()){
            //     Debug.Log("Waiting for wave");
            //     yield return new WaitForFixedUpdate();
            // }
            yield return new WaitForFixedUpdate();
            if (friends.Count < maxSoldiers)
            {
                summoningCircle.StartSummon(generateSpeed);
                yield return new WaitForSeconds(generateSpeed * localTimeScale);
                SpawnSoldier();
            }
        }
    }

    void SpawnSoldier(){
        int index = Random.Range(0, spawnLocations.Length);
        FriendlyAI newSoldier = Instantiate(soldier, spawnLocations[index].transform.position, Quaternion.identity, gameObject.transform);
        friends.Add(newSoldier);
        SoundFXManager.instance.PlaySoundFXClip(creationSound, transform, 1);
    }

    public void RemoveSoldier(FriendlyAI toRemove){
        Debug.Log("Removing a fella");
        friends.Remove(toRemove);
    }
}
