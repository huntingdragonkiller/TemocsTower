using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurretUIManager : TowerUIManager
{
    public TextMeshProUGUI damage;
    public TextMeshProUGUI attackSpeed;

    

    public void UpdateDamage(int newDamage){
        damage.text = "Damage: " + newDamage.ToString();
    }

    public void UpdateAttackSpeed(float newAttackSpeed){
        attackSpeed.text = "Attack Speed: " + newAttackSpeed.ToString() + " Sec";
    }

}
