using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{

    public List<GameObject> spellPrefabs;
    public int manaAmount;
    public int maxMana;

    [HideInInspector]
    public bool spellReady;

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
        spellReady = false;
    }

    // Update is called once per frame
    void Update()
    {

        checkIfSpellSelected();

        if (spellReady) {
            // call get mouse click coordinates  ?
            // on Mouse Click:
            //  get mouse click coordinates
            //  get coordinates of where fireball is gonna spawn from (either just outside top right
            //      corner of camera or top left corner of camera depending on mouse click position) 
        }
    }

    public void checkIfSpellSelected() {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || 
            Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || 
            Input.GetKeyDown(KeyCode.Alpha5)) {

            spellReady = true;
        }
    }

    public void addToMana(int manaToAdd) {
        if (manaAmount + manaToAdd >= maxMana) {
            manaAmount = maxMana;
        } else {
            manaAmount += manaToAdd;
        }
    }

    public void decreaseMana(int manaCost) {
        if (manaAmount - manaCost <= 0) {
            manaAmount = 0;
        } else {
            manaAmount -= manaCost;
        }
    }

    // on '1' key press, spellReady = true;


}
