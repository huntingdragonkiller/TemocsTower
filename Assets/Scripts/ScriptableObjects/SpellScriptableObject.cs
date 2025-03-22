using UnityEngine;

[CreateAssetMenu(fileName = "SpellScriptableObject", menuName = "ScriptableObjects/Spell")]
public class SpellScriptableObject : ScriptableObject
{
    //Base stats for towers
    // [SerializeField] GameObject spellPrefab;
    // public GameObject SpellPrefab {get => spellPrefab; set => spellPrefab = value; } 

    [SerializeField] public float duration;
    public float Duration {get => duration; set => duration = value; }

    [SerializeField] public float damage; // can be null due to buffs and shield and the sort
    public float Damage {get => damage; set => damage = value; }

    [SerializeField] public float cooldownTime;
    public float CooldownTime {get => cooldownTime; set => cooldownTime = value; }

    [SerializeField] public int manaCost;
    public int ManaCost { get => manaCost; set => manaCost = value; }
}
