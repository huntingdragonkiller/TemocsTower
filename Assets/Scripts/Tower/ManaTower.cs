using UnityEngine;
using System.Collections;

public class ManaTower : TowerSegment
{
    public float generateAmount;
    public float generateInterval;
    private IEnumerator generateMana;

    private void Awake()
    {
        generateMana = GenerateMana(generateInterval);
        StartCoroutine(generateMana);

    }

    // attacks at an interval given by the attack speed stat
    protected virtual IEnumerator GenerateMana(float waitTime)
    {
        while (true)
        {
            Debug.Log("Generated " + generateAmount + " mana");
            yield return new WaitForSeconds(waitTime);
        }
    }
}
