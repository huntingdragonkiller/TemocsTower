using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public TowerSegment baseSegment; // Starting segment of the tower
    public List<TowerSegment> towerSegments = new List<TowerSegment>();
    public float dropDownAnimationTime = .1f;

    private void Start()
    {
        towerSegments.Add(baseSegment);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DestroyRandomSegment();
        }

        
    }

    public void AddSegment(TowerSegment newSegment)
    {
        TowerSegment topSegment = towerSegments[towerSegments.Count - 1];
        newSegment.AttachTo(topSegment);
        towerSegments.Add(newSegment);
    }

    public void DestroyRandomSegment()
    {
        if (towerSegments.Count <= 1) return;
        
        int randomIndex = Random.Range(1, towerSegments.Count);
        TowerSegment randomSegment = towerSegments[randomIndex];
        
        towerSegments.RemoveAt(randomIndex);
        Destroy(randomSegment.gameObject);

        StartCoroutine(ShiftSegmentsDown());
    }

    public void HealAllSegments(){
        foreach (TowerSegment segment in towerSegments){
            segment.FullHeal();
        }
    }

    public void DestroySegment(TowerSegment segment)
    {
        towerSegments.Remove(segment);
        Destroy(segment.gameObject);
        StartCoroutine(ShiftSegmentsDown());
    }

    private IEnumerator ShiftSegmentsDown()
    {
        
        //Setting the tower at the bottom to the lowest position (will do nothing if its already there)
        if(towerSegments[0].gameObject.transform.position != new Vector3(0, 2, 0)){
            Vector3 startPos = towerSegments[0].gameObject.transform.position;
            Vector3 targetPos = new Vector3(0, 2, 0);
            int animationFrames = (int) (dropDownAnimationTime / Time.fixedDeltaTime);
            int elapsedFrames = 0;
            while (elapsedFrames < animationFrames)
            {
                float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
                towerSegments[0].gameObject.transform.position = Vector3.Lerp(startPos, targetPos, interpolationRatio);
                elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
                yield return new WaitForFixedUpdate();
            }
        }
        for (int i = 1; i < towerSegments.Count; i++) // Skip base segment
        {
            TowerSegment below = towerSegments[i-1];
            TowerSegment current = towerSegments[i];
            //if they're already next to each other, we skip the animation
            if(current.transform.position.y - below.transform.position.y == 6){
                continue;
            }
            
            Vector3 startPos = current.transform.position;
            Vector3 targetPos = below.attachmentPoint.position;

            int animationFrames = (int) (dropDownAnimationTime / Time.fixedDeltaTime);
            int elapsedFrames = 0;
            while (elapsedFrames < animationFrames)
            {
                float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
                current.transform.position = Vector3.Lerp(startPos, targetPos, interpolationRatio);
                elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
                yield return new WaitForFixedUpdate();
            }

            current.transform.position = targetPos; // Snap to final position
            current.belowSegment = below;
        }
    }
}
