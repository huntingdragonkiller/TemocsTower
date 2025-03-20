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
    GameObject currentItem;
   
    void Awake()
    {
        shopManager = GetComponentInParent<ShopManager>();
    }

    public void SetNewItem(Sprite image, string name, string description, int cost, int rerollCost, GameObject item){
        previewImage.sprite = image;
        itemName.text = name;
        itemDescription.text = description;
        itemCost.text = cost.ToString();
        this.cost = cost;
        this.rerollCost.text = rerollCost.ToString();
        currentRerollCost = rerollCost;
        currentItem = item;
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
