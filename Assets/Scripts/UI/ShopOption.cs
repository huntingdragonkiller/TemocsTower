using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopOption : MonoBehaviour
{
    [SerializeField]
    Image previewImage;
    [SerializeField]
    TextMeshProUGUI itemName;
    [SerializeField]
    TextMeshProUGUI itemDescription;
    [SerializeField]
    TextMeshProUGUI itemCost;
    [SerializeField]
    TextMeshProUGUI rerollCost;
    [SerializeField]
    public bool canHaveTowers;
    int cost;
    int currentRerollCost;
    ShopManager shopManager;
    SelectionScriptableObject currentItem;
   
    void Awake()
    {
        shopManager = GetComponentInParent<ShopManager>();
    }

    // Sets the display for the passed selection, if it can display it
    // Returns true if it was able to successfully change the tower
    // Returns false if it can't display it
    public bool SetNewItem(SelectionScriptableObject selection, int rerollCost){
        if(!CheckIfValidSelection(selection))
            return false;
        previewImage.sprite = selection.Sprite;
        itemName.text = selection.Name;
        itemDescription.text = selection.Description;
        itemCost.text = cost.ToString();
        cost = selection.Cost;
        this.rerollCost.text = rerollCost.ToString();
        currentRerollCost = rerollCost;
        currentItem = selection;
        return true;
    }

    bool CheckIfValidSelection(SelectionScriptableObject toCheck){
        //If its a tower return true/false if it can/can't display towers
        if(toCheck.Selection.GetComponent<TowerSegment>() != null){
            return canHaveTowers;
        } 
        else return true;
    }

    public void RequestReroll(){
        if(FindFirstObjectByType<CoinManager>().SpendCoins(currentRerollCost))
        {
            shopManager.Reroll(this, currentItem, currentRerollCost);
        }
    }

    public void RequestPurchase(){
        if(FindFirstObjectByType<CoinManager>().SpendCoins(cost))
        {
            shopManager.Purchase(this, currentItem);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
