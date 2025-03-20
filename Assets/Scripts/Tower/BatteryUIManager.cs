using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatteryUIManager : TowerUIManager
{
    public TextMeshProUGUI manaStorage;
    public TextMeshProUGUI friendlyDamage;

    

    public void UpdateStorage(float manaStorage){
        this.manaStorage.text = "Mana Storage: +" + manaStorage.ToString();
    }

    public void UpdateFriendlyDamage(float friendlyDamage, float baseDamage){
        this.friendlyDamage.text = "Friendly Damage: " + friendlyDamage.ToString() + "(originally: " + baseDamage.ToString() + ")";
    }

}
