using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    public RectTransform healthBar;
    public float initialWidth;
    public float animationTime;
    public float newWidth;
    int animationFrames;
    int elapsedFrames = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        healthBar = GetComponent<RectTransform>();
        initialWidth =  healthBar.rect.width;
        newWidth = initialWidth;
        animationFrames = (int) (animationTime / Time.fixedDeltaTime);  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
        healthBar.sizeDelta = Vector2.Lerp(healthBar.sizeDelta, new Vector2(newWidth, healthBar.rect.height), interpolationRatio);
        elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
    }

    public void UpdateHealth(float percentHealth){
        newWidth = initialWidth * percentHealth;
    }
}
