using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class ManaTower : TowerSegment
{
    [SerializeField]
    AudioResource manaGenSound;
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
            // Disables coroutine while there isn't a wave
             while(!FindAnyObjectByType<EnemyManager>().GetActiveWave()){
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForSeconds(waitTime * localTimeScale);
            Debug.Log("Generated " + generateAmount + " mana");
            SoundFXManager.instance.PlaySoundFXClip(manaGenSound, transform, 1);
            
        }
    }
}
