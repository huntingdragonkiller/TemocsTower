using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public float animationTime = 1.5f;
    [SerializeField]
    public int rerollCost = 20;
    [SerializeField]
    public float rerollIncrease = 1.5f;
    public RectTransform shopRect;
    float initialWidth;
    float initialHeight;
    float enlargedWidth;
    float enlargedHeight;
    int animationFrames;
    int elapsedFrames = 0;
    bool hovering = false;
    bool shopLocked = true;
    List<TowerSegment> availableSegments = new List<TowerSegment>();
    ShopOption[] shopOptions;
    
    
     void Awake()
    {
        shopRect = GameObject.Find("Shop").GetComponent<RectTransform>();
        Debug.Log("" + shopRect.sizeDelta);
        initialWidth =  shopRect.rect.width;
        initialHeight =  shopRect.rect.height;
        RectTransform enlargedRect = GameObject.Find("ShopBackground").GetComponent<RectTransform>();
        Debug.Log("" + enlargedRect.gameObject);
        enlargedHeight = enlargedRect.rect.height;
        enlargedWidth = enlargedRect.rect.width;
        animationFrames = (int) (animationTime / Time.fixedDeltaTime); 
        shopOptions = GetComponentsInChildren<ShopOption>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (availableSegments.Count >= shopOptions.Length && shopLocked)
        {
            InitializeShop();
        }
        if(hovering && !shopLocked){
            float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
            shopRect.sizeDelta = Vector2.Lerp(shopRect.sizeDelta, new Vector2(enlargedWidth, enlargedHeight), interpolationRatio);
            elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
        } else {
            float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
            shopRect.sizeDelta = Vector2.Lerp(shopRect.sizeDelta, new Vector2(initialWidth, initialHeight), interpolationRatio);
            elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
        }
    }
    public void InitializeShop(){
        List<GameObject> previousOptions = new List<GameObject>();
        foreach(ShopOption option in shopOptions){
            if(option.canHaveTowers){
                TowerSegment toAdd = PickRandomTower();
                while(previousOptions.Contains(toAdd.gameObject)){
                    toAdd = PickRandomTower();
                    Debug.Log("Shuffle!");
                }
                previousOptions.Add(toAdd.gameObject);
                option.SetNewItem(toAdd.towerData.Sprite, toAdd.towerData.name, toAdd.towerData.Description, toAdd.towerData.PurchaseCost, rerollCost, toAdd.gameObject);
            }
        }
        shopLocked = false;
    }
    public void Reroll(ShopOption toReroll, GameObject currentItem, int currentRerollCost){
        if(toReroll.canHaveTowers){
            TowerSegment toAdd = PickRandomTower();
            while(toAdd.Equals(currentItem)){
                toAdd = PickRandomTower();
                Debug.Log("Shuffle!");
            }
            int newRerollCost = (currentRerollCost == 0) ? rerollCost : (int) (currentRerollCost * rerollIncrease);
            toReroll.SetNewItem(toAdd.towerData.Sprite, toAdd.towerData.name, toAdd.towerData.Description, toAdd.towerData.PurchaseCost, newRerollCost, toAdd.gameObject);
        }
    } 
    public void Purchase(ShopOption purchasing, GameObject currentItem){
        TowerSegment tower = currentItem.GetComponent<TowerSegment>();
        if(tower != null){
            TowerSegment addingTower = Instantiate(tower);
            addingTower.enabled = true;
            FindFirstObjectByType<TowerManager>().AddSegment(addingTower);
        } else{
            //Add logic for purchasing other items besides towers
        }
        Reroll(purchasing, currentItem, 0);
    }
    public void AddNewTower(TowerSegment toAdd){
        TowerSegment shopReference = Instantiate(toAdd, new Vector3(10000,10000,10000), Quaternion.identity, gameObject.transform);
        availableSegments.Add(shopReference);
        shopReference.enabled = false;
    }
    TowerSegment PickRandomTower(){
        return availableSegments[Random.Range(0,availableSegments.Count)];
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        elapsedFrames = 0;
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        elapsedFrames = 0;
        hovering = false;
    }
}
