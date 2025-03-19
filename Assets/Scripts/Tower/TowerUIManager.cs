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
    public RectTransform towerRect;
    public int initialWidth;
    public int initialHeight;
    public int enlargedWidth;
    public int enlargedHeight;
    public float animationTime;

    int animationFrames;
    int elapsedFrames = 0;
    bool hovering = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animationFrames = (int) (animationTime / Time.fixedDeltaTime);
        Debug.Log(towerRect.sizeDelta);
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
        health.text = "Health: " + newHealth.ToString() + " / " +  maxHealth.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
