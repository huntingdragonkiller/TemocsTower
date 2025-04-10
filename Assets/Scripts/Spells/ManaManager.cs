using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManaManager : MonoBehaviour
{
    [SerializeField]
    ManaUIManager manaUIManager;
    [SerializeField]
    TextMeshProUGUI manaAmountText;
    [SerializeField]
    float passiveManaGenSpeed = 1f;
    [SerializeField]
    int passiveManaGenAmount = 1;
    public List<Spell> spellPrefabs;
    List<bool> canCastSpell = new List<bool>();
    public int manaAmount;
    public int maxMana;

    [HideInInspector]
    public bool spellReady;
    Spell preparedSpell;
    IEnumerator resetCastSpell;
    IEnumerator passiveManaGen;
    int spellIndex = -1;

    //public SpellScriptableGameObject currentSpell;

    public static ManaManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(Spell spell in spellPrefabs){
            canCastSpell.Add(true);
        }
        spellReady = false;
        passiveManaGen = PassiveManaGeneration();
        StartCoroutine(passiveManaGen);
    }

    private IEnumerator PassiveManaGeneration()
    {
        while (true)
        {
            addToMana(passiveManaGenAmount);
            yield return new WaitForSeconds(passiveManaGenSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {

        checkIfSpellSelected();

        if(spellIndex == -1)
        {
            return;
        }
        if (canCastSpell[spellIndex] && Input.GetMouseButtonDown(0) && Time.timeScale > 0)
        {
            // call get mouse click coordinates  ?
            // on Mouse Click:
            //  get mouse click coordinates
            //  get coordinates of where fireball is gonna spawn from (either just outside top right
            //      corner of camera or top left corner of camera depending on mouse click position) 

            // Get the mouse position in screen space
            Vector3 mouseScreenPosition = Input.mousePosition;

            // Set the Z position to be the camera's Z position (or a fixed value for 2D)
            mouseScreenPosition.z = 0; // You can also use a fixed value like 0 if needed

            // Convert the screen position to world position
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            mouseWorldPosition.z = 0;// need this to see the spell + have it actually work

            // Output the world position (x, y)
            Debug.Log("Mouse World Position: " + new Vector2(mouseWorldPosition.x, mouseWorldPosition.y));
            
            Spell castedSpell = Instantiate(preparedSpell);
            if(decreaseMana(castedSpell.manaCost)){
                castedSpell.CastSpellAt(mouseWorldPosition);
                canCastSpell[spellIndex] = false;
                resetCastSpell = WaitTillDone(manaUIManager.CastedSelectedSpell(), spellIndex);
                StartCoroutine(resetCastSpell);
                spellIndex = -1;
                preparedSpell = null;
            } else {
                Destroy(castedSpell);
            }

            spellReady = false;
        }
    }

    private IEnumerator WaitTillDone(SpellUIManager spellUIManager, int index)
    {

        while(spellUIManager.onCooldown){
            yield return new WaitForFixedUpdate();
            Debug.Log("still waiting");
        }
        canCastSpell[index] = true;
        Debug.Log("We're good");
    }

    public void checkIfSpellSelected()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spellIndex = 0;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            spellIndex = 1;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            
            spellIndex = 2;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
            spellIndex = 3;
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5)){
    
            spellIndex = 4;
        
            
        }

        if(spellIndex != -1){
            preparedSpell = spellPrefabs[spellIndex];
            spellReady = true;
            manaUIManager.SetActiveSpell(spellIndex);
        }
           
    }

    public void addToMana(int manaToAdd)
    {
        if (manaAmount + manaToAdd >= maxMana)
        {
            manaAmount = maxMana;
            manaAmountText.text = "Mana: " + manaAmount;
        }
        else
        {
            manaAmount += manaToAdd;
            manaAmountText.text = "Mana: " + manaAmount;
        }
    }

    public void IncreaseMaxMana(int amountToIncrease)
    {
        maxMana += amountToIncrease;
        if (manaAmount > maxMana)
            manaAmount = maxMana;
        manaAmountText.text = "Mana: " + manaAmount;
    }


    //Returns true/false if it was able/unable to successfully spend the mana (ie. had enough mana) 
    public bool decreaseMana(int manaCost)
    {
        if (manaAmount - manaCost < 0)
        {
            return false;
        }
        else
        {
            manaAmount -= manaCost;
            manaAmountText.text = "Mana: " + manaAmount;
            return true;
        }
    }



}
