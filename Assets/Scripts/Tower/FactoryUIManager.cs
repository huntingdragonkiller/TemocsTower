using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUIManager : TowerUIManager
{
    public TextMeshProUGUI maxSoldiers;
    public TextMeshProUGUI productionSpeed;

    

    public void UpdateMaxSoldiers(int newMaxSoldiers){
        maxSoldiers.text = "Max Soldiers: " + newMaxSoldiers.ToString();
    }

    public void UpdateProdSpeed(float newProdSpeed){
        productionSpeed.text = "Production Speed: " +newProdSpeed.ToString() + " Sec";
    }

}
