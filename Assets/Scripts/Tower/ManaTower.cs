using UnityEngine;
using System.Collections;

public class ManaTower : TowerSegment
{
    public float generateAmount;
    public float generateInterval;
    private IEnumerator generateMana;
    public ManaUIManager manaUIManager;

    public override void Awake()
    {
        base.Awake();
        generateMana = GenerateMana(generateInterval);
        StartCoroutine(generateMana);
        manaUIManager.UpdateManaGenerated((int)generateAmount);
        manaUIManager.UpdateProdSpeed(generateInterval);

    }

    public override bool Upgrade()
    {
        if(!base.Upgrade())
            return false;
        
        generateAmount += 5;
        generateInterval -= 0.1f;
        RestartManaCoroutine();
        manaUIManager.UpdateManaGenerated((int)generateAmount);
        manaUIManager.UpdateProdSpeed(generateInterval);
        return true;
    }

    void RestartManaCoroutine(){
        StopCoroutine(generateMana);
        generateMana = GenerateMana(generateInterval);
        StartCoroutine(generateMana);
    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator GenerateMana(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            Debug.Log("Generated " + generateAmount + " mana");
            
        }
    }
}
