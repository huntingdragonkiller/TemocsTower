using UnityEngine;
using System.Collections;

public class GoldTower : TowerSegment
{
    public int generateAmount;
    public float generateInterval;
    private IEnumerator generateGold;
    GoldUIManager goldUIManager;
    CoinManager coinManager;


    public override void Awake()
    {
        base.Awake();
        goldUIManager = GetComponentInChildren<GoldUIManager>();
        coinManager = FindFirstObjectByType<CoinManager>();
        generateGold = GenerateGold(generateInterval);
        StartCoroutine(generateGold);
        goldUIManager.UpdateGoldGenerated((int)generateAmount);
        goldUIManager.UpdateProdSpeed(generateInterval);

    }

    public override bool Upgrade()
    {
        if(!base.Upgrade())
            return false;
        
        generateAmount += 5;
        generateInterval -= 0.1f;
        RestartGoldCoroutine();
        goldUIManager.UpdateGoldGenerated((int)generateAmount);
        goldUIManager.UpdateProdSpeed(generateInterval);
        return true;
    }

    void RestartGoldCoroutine(){
        StopCoroutine(generateGold);
        generateGold = GenerateGold(generateInterval);
        StartCoroutine(generateGold);
    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator GenerateGold(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            coinManager.AddCoins((generateAmount));
            Debug.Log("Generated " + generateAmount + " gold");
            
        }
    }
}
