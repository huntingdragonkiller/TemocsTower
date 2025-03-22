using System.Collections.Generic;
using UnityEngine;

public class ManaManager : MonoBehaviour
{

    public List<Spell> spellPrefabs;
    public int manaAmount;
    public int maxMana;

    [HideInInspector]
    public bool spellReady;
    Spell preparedSpell;

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
        spellReady = false;
    }

    // Update is called once per frame
    void Update()
    {

        checkIfSpellSelected();

        // if 
        if (spellReady && Input.GetMouseButtonDown(0))
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
            } else {
                Destroy(castedSpell);
            }

            spellReady = false;
        }
    }

    public void checkIfSpellSelected()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            try{preparedSpell = spellPrefabs[0];
            spellReady = true;}
            catch{}
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            try{preparedSpell = spellPrefabs[1];
            spellReady = true;}
            catch{}
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            try{preparedSpell = spellPrefabs[2];
            spellReady = true;}
            catch{}
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            try{preparedSpell = spellPrefabs[3];
            spellReady = true;}
            catch{}
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            try{preparedSpell = spellPrefabs[4];
            spellReady = true;}
            catch{}
            
            
        } else {
        }
    }

    public void addToMana(int manaToAdd)
    {
        if (manaAmount + manaToAdd >= maxMana)
        {
            manaAmount = maxMana;
        }
        else
        {
            manaAmount += manaToAdd;
        }
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
            return true;
        }
    }



}
