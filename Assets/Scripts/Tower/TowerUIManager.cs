using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerUIManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI upgradeCost;
    public TextMeshProUGUI health;

    public TowerScriptableObject stats;
    RectTransform towerRect;
    float initialWidth;
    float initialHeight;
    float enlargedWidth;
    float enlargedHeight;
    float animationTime = 1.25f;

    int animationFrames;
    int elapsedFrames = 0;
    bool hovering = false;

    void Awake()
    {
        towerRect = GetComponent<RectTransform>();
        initialHeight = towerRect.rect.height;
        initialWidth = towerRect.rect.width;
        RectTransform maximizedRect = GameObject.Find("Panel").GetComponent<RectTransform>();
        enlargedHeight = maximizedRect.rect.height;
        enlargedWidth = maximizedRect.rect.width;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animationFrames = (int) (animationTime / Time.fixedDeltaTime);
        Debug.Log(towerRect.sizeDelta);
        Debug.Log(towerRect.rect.height);
        Debug.Log(towerRect.rect.width);
        Debug.Log(enlargedHeight);
        Debug.Log(enlargedWidth);
        // towerRect.sizeDelta = new Vector2(enlargedHeight, enlargedWidth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hovering){
            float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
            towerRect.sizeDelta = Vector2.Lerp(towerRect.sizeDelta, new Vector2(enlargedWidth, enlargedHeight), interpolationRatio);
            elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
        } else {
            float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
            towerRect.sizeDelta = Vector2.Lerp(towerRect.sizeDelta, new Vector2(initialWidth, initialHeight), interpolationRatio);
            elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
        }
    }

    public void UpdateLevel(int newLevel){
        level.text = newLevel.ToString();
    }

    public void UpdateCost(int newCost){
        upgradeCost.text = "Upgrade: " + newCost.ToString() + " Gold";
    }

    public void UpdateHealth(float newHealth, float maxHealth){
        health.text = "Health: " +((int) newHealth).ToString() + " / " +  ((int)maxHealth).ToString();
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
