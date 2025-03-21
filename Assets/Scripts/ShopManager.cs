using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    List<SelectionScriptableObject> availableSelections = new List<SelectionScriptableObject>();
    ShopOption[] shopOptions;
    Vector2 initialSizeDelta;
    Vector2 enlargedSizeDelta;
    
    
     void Awake()
    {
        shopRect = GameObject.Find("Shop").GetComponent<RectTransform>();
        Debug.Log("" + shopRect.sizeDelta);
        initialWidth =  shopRect.rect.width;
        initialHeight =  shopRect.rect.height;
        initialSizeDelta = shopRect.sizeDelta;
        RectTransform enlargedRect = GameObject.Find("BackgroundPanel").GetComponent<RectTransform>();
        Debug.Log("" + enlargedRect.sizeDelta);
        enlargedHeight = enlargedRect.rect.height;
        enlargedWidth = enlargedRect.rect.width;
        enlargedSizeDelta = enlargedRect.sizeDelta;
        animationFrames = (int) (animationTime / Time.fixedDeltaTime); 
        shopOptions = GetComponentsInChildren<ShopOption>();
    }
    void Start(){
        // InitializeShop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (availableSelections.Count > 0 && shopLocked)
        {
            InitializeShop();
        }
        if(hovering){
            Debug.Log("Expanding");
            float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
            shopRect.sizeDelta = Vector2.Lerp(shopRect.sizeDelta, enlargedSizeDelta, interpolationRatio);
            elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
        } else {
            float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
            shopRect.sizeDelta = Vector2.Lerp(shopRect.sizeDelta, initialSizeDelta, interpolationRatio);
            
            Debug.Log("Shrinking");
            elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
        }
        Debug.Log(shopRect.sizeDelta);
    }
    public void InitializeShop(){
        List<GameObject> previousOptions = new List<GameObject>();
        foreach(ShopOption option in shopOptions){
            

            //loops until a valid choice has been found randomly
            while(!option.SetNewItem(PickRandomSelection(), rerollCost)){}

            // if(option.canHaveTowers){
            //     TowerSegment toAdd = PickRandomTower();
            //     while(previousOptions.Contains(toAdd.gameObject)){
            //         toAdd = PickRandomTower();
            //         Debug.Log("Shuffle!");
            //     }
            //     previousOptions.Add(toAdd.gameObject);
            //     option.SetNewItem(toAdd.towerData.Sprite, toAdd.towerData.name, toAdd.towerData.Description, toAdd.towerData.PurchaseCost, rerollCost, toAdd.gameObject);
            // }
        }
        shopLocked = false;
    }

    private SelectionScriptableObject PickRandomSelection()
    {
        return availableSelections[Random.Range(0, availableSelections.Count)];
    }

    public void Reroll(ShopOption toReroll, SelectionScriptableObject currentItem, int currentRerollCost){
        SelectionScriptableObject newSelection = PickRandomSelection();
        while(newSelection == currentItem){
            if(availableSelections.Count == 1)
                break;
            newSelection = PickRandomSelection();
        }


        int newRerollCost = (currentRerollCost == 0) ? rerollCost : (int) (currentRerollCost * rerollIncrease);
        toReroll.SetNewItem(newSelection, newRerollCost);
    } 
    public void Purchase(ShopOption purchasing, SelectionScriptableObject currentItem){


        //Treat it like a selection cause it is
        FindFirstObjectByType<SelectionManager>().HandleChosenSelection(currentItem);

        Reroll(purchasing, currentItem, 0);

        // TowerSegment tower = currentItem.GetComponent<TowerSegment>();
        // if(tower != null){
        //     TowerSegment addingTower = Instantiate(tower);
        //     addingTower.enabled = true;
        //     FindFirstObjectByType<TowerManager>().AddSegment(addingTower);
        // } else{
        //     //Add logic for purchasing other items besides towers
        // }
    }

    public void AddNewSelection(SelectionScriptableObject toAdd){
        if(!availableSelections.Contains(toAdd))
            availableSelections.Add(toAdd);
    }
    // public void AddNewTower(TowerSegment toAdd){
    //     TowerSegment shopReference = Instantiate(toAdd, new Vector3(10000,10000,10000), Quaternion.identity, gameObject.transform);
    //     availableSelections.Add(shopReference);
    //     shopReference.enabled = false;
    // }
    // TowerSegment PickRandomTower(){
    //     return availableSelections[Random.Range(0,availableSelections.Count)];
    // }

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
