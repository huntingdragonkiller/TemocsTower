using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{

    
    public List<GameObject> spellPrefabs;
    public int manaAmount;

    public static ManaManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    // vars: currentSpell (ScriptableGameObject)
    // string for UI reasons mostly. XXXXXXX
    // addToMana
    // decreaseMana
    // castSpell(currentSpellPrefab)
    // something like that. Thoughts ? 


    // mana cost
    // cooldown
    // how much damage is dealt (can be null (mana shield))

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
