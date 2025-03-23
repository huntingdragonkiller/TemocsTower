using System;
using UnityEngine;

public class ManaUIManager : MonoBehaviour
{
    [SerializeField]
    SpellUIManager[] spellUIManagers;
    [SerializeField]
    private float offset = 50f;
    SpellUIManager currentSelectedSpell;

    public void SetActiveSpell(int index){
        DeselectCurrentSpell();
        currentSelectedSpell = spellUIManagers[index];
        RectTransform box = currentSelectedSpell.gameObject.GetComponent<RectTransform>();
        Vector2 leftoffset = box.offsetMax;
        Vector2 rightoffset = box.offsetMin;
        leftoffset.x += offset;
        rightoffset.x += offset;
        box.offsetMax = leftoffset;
        box.offsetMin = rightoffset;
    }

    public void DeselectCurrentSpell()
    {
        if (currentSelectedSpell == null)
            return;
        RectTransform box = currentSelectedSpell.gameObject.GetComponent<RectTransform>();
        Vector2 leftoffset = box.offsetMax;
        Vector2 rightoffset = box.offsetMin;
        leftoffset.x -= offset;
        rightoffset.x -= offset;
        box.offsetMax = leftoffset;
        box.offsetMin = rightoffset;
    }

    public SpellUIManager CastedSelectedSpell(){
        DeselectCurrentSpell();
        currentSelectedSpell.ActivateCooldown();
        SpellUIManager toReturn = currentSelectedSpell;
        currentSelectedSpell=null;
        return toReturn;
    }
}
