using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIManager : TowerUIManager
{
    public TextMeshProUGUI goldGenerated;
    public TextMeshProUGUI productionSpeed;

    

    public void UpdateGoldGenerated(int newGoldGenerated){
        goldGenerated.text = "Gold Generated: " + newGoldGenerated.ToString();
    }

    public void UpdateProdSpeed(float newProdSpeed){
        productionSpeed.text = "Production Speed: " +newProdSpeed.ToString() + " Sec";
    }

}
