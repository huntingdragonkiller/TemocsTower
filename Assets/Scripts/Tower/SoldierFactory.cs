using UnityEngine;
using System.Collections;

public class SoldierFactory : TowerSegment
{
    public GameObject[] spawnLocations;

    public FriendlyAI soldier;
    public float generateSpeed;
    private IEnumerator createSoldier;

    private void Awake()
    {
        createSoldier = CreateSoldier(generateSpeed);
        StartCoroutine(createSoldier);

    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator CreateSoldier(float waitTime)
    {
        while (true)
        {
            int index = Random.Range(0, spawnLocations.Length);
            FriendlyAI newSoldier = Instantiate(soldier, spawnLocations[index].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
