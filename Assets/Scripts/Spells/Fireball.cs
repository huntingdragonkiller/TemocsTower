using UnityEngine;

public class Fireball : MonoBehaviour
{

    public SpellScriptableObject spellData;
    private float damage;
    private float cooldownTime;
    private int manaCost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damage = spellData.damage;
        cooldownTime = spellData.cooldownTime;
        manaCost = spellData.manaCost;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
