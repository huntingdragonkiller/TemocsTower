using UnityEngine;

[CreateAssetMenu(fileName = "SpellScriptableObject", menuName = "ScriptableObjects/Spell")]
public class SpellScriptableObject : ScriptableObject
{
    //Base stats for towers
    // [SerializeField] GameObject spellPrefab;
    // public GameObject SpellPrefab {get => spellPrefab; set => spellPrefab = value; } 

    [SerializeField] float cooldownTime;
    public float CooldownTime {get => cooldownTime; set => cooldownTime = value; }

    [SerializeField] int manaCost;
    public int ManaCost { get => manaCost; set => manaCost = value; }
}
