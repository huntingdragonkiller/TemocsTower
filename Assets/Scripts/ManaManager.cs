using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{

    public List<GameObject> spellPrefabs;
    public int manaAmount;
    public int maxMana;
    //public SpellScriptableGameObject currentSpell;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void addToMana(int manaToAdd) {
        if (manaAmount + manaToAdd >= maxMana) {
            manaAmount = maxMana;
        } else {
            manaAmount += manaToAdd;
        }
    }

    public static void decreaseMana(int manaCost) {
        if (manaAmount - manaCost <= 0) {
            manaAmount = 0;
        } else {
            manaAmount -= manaCost;
        }
    }


}
