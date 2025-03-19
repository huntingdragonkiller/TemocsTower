using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierFactory : TowerSegment
{
    public GameObject[] spawnLocations;
    public FactoryUIManager factoryUIManager;

    public FriendlyAI soldier;
    public float generateSpeed;
    private IEnumerator createSoldier;
    List<FriendlyAI> friends = new List<FriendlyAI>();
    [SerializeField]
    private int maxSoldiers;

    public override void Awake()
    {
        base.Awake();
        createSoldier = CreateSoldier(generateSpeed);
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
    protected virtual IEnumerator CreateSoldier(float waitTime)
    {
        while (true)
        {
            if (friends.Count < maxSoldiers)
            {
                int index = Random.Range(0, spawnLocations.Length);
                FriendlyAI newSoldier = Instantiate(soldier, spawnLocations[index].transform.position, Quaternion.identity, gameObject.transform);
                friends.Add(newSoldier);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
