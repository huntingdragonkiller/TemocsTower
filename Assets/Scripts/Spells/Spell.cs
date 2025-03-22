using UnityEngine;

public class Spell : MonoBehaviour
{
    [SerializeField]
    public SpellScriptableObject spellData;
    [SerializeField]
    Vector3 spawnPosition;//Where the spell itself should spawn in relation to where the spell was cast
    protected Vector3 targetPosition;//Where the spell is targeted
    protected float damage;
    protected float duration;
    protected float cooldownTime;
    public int manaCost;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        damage = spellData.damage;
        duration = spellData.duration;
        cooldownTime = spellData.cooldownTime;
        manaCost = spellData.manaCost;
    }

    public virtual void CastSpellAt(Vector3 position){
        targetPosition = position;
        transform.position = targetPosition + spawnPosition;
    }
}
