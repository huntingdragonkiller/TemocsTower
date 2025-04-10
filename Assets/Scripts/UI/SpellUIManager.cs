using System.Collections;
using UnityEngine;

public class SpellUIManager : MonoBehaviour
{
    [SerializeField]
    SpellScriptableObject spell;
    [SerializeField]
    RectTransform cooldownBar;
    float coolDownTime;
    float initialHeight;
    public bool onCooldown = false;
    void Awake()
    {
        coolDownTime = spell.CooldownTime;
        initialHeight = cooldownBar.rect.height;

    }

    public void ActivateCooldown(){
        onCooldown = true;
        StartCoroutine(coolDown());
    }

    IEnumerator coolDown(){
        Debug.Log("Cooling");
        float originalMaxY = cooldownBar.anchorMax.y;
        float anchorDiff = cooldownBar.anchorMax.y - cooldownBar.anchorMin.y;
        float percentPerFrame = anchorDiff * Time.deltaTime / coolDownTime;
        Vector2 offset = new Vector2(0, percentPerFrame);
        cooldownBar.anchorMax = new Vector2(cooldownBar.anchorMax.x, cooldownBar.anchorMin.y);
        while(cooldownBar.anchorMax.y < originalMaxY){
            yield return new WaitForFixedUpdate();
            
            cooldownBar.anchorMax += offset;
            //Debug.Log(cooldownBar.anchorMax);
        }
        Debug.Log("DONE");
        onCooldown = false;
    }


}
