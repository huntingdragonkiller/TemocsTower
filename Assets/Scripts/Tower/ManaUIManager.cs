using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaUIManager : TowerUIManager
{
    public TextMeshProUGUI manaGenerated;
    public TextMeshProUGUI productionSpeed;

    

    public void UpdateManaGenerated(int newManaGenerated){
        manaGenerated.text = "Mana Generated: " + newManaGenerated.ToString();
    }

    public void UpdateProdSpeed(float newProdSpeed){
        productionSpeed.text = "Production Speed: " +newProdSpeed.ToString() + " Sec";
    }

}
