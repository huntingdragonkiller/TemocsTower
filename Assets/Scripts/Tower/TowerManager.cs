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

    public void AddSegment(TowerSegment newSegment)
    {
        TowerSegment topSegment = towerSegments[towerSegments.Count - 1];
        newSegment.AttachTo(topSegment);
        towerSegments.Add(newSegment);
    }
}
