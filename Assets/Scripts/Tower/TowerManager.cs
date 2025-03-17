using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class TowerManager : MonoBehaviour
{
    public TowerSegment baseSegment; // Starting segment of the tower
    public List<TowerSegment> towerSegments = new List<TowerSegment>();

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

    public void DestroySegment(TowerSegment segment)
    {
        towerSegments.Remove(segment);
        Destroy(segment.gameObject);
        StartCoroutine(ShiftSegmentsDown());
    }

    private IEnumerator ShiftSegmentsDown()
    {
        float duration = 0.05f; // Adjust for smoother/faster falling
        float elapsedTime;
        
        //Setting the tower at the bottom to the lowest position (will do nothing if its already there)
        if(towerSegments[0].gameObject.transform.position != new Vector3(0, 2, 0)){
            Vector3 startPos = towerSegments[0].gameObject.transform.position;
            Vector3 targetPos = new Vector3(0, 2, 0);
            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                towerSegments[0].gameObject.transform.position = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }
        }
        for (int i = 1; i < towerSegments.Count; i++) // Skip base segment
        {
            TowerSegment below = towerSegments[i-1];
            TowerSegment current = towerSegments[i];
            
            Vector3 startPos = current.transform.position;
            Vector3 targetPos = below.attachmentPoint.position;
            
            elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                current.transform.position = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }

            current.transform.position = targetPos; // Snap to final position
            current.belowSegment = below;
        }
    }
}
