using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SegmentSelectionManager : MonoBehaviour
{
    public TowerManager towerManager; // Reference to the tower
    public GameObject menuOverlay;
    public List<TowerSegment> availableSegments; // Pool of possible segments

    public Button[] optionButtons; // UI buttons for choosing segments
    private TowerSegment[] _chosenSegments; // The three options
    void Start()
    {
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false); // Hide buttons on start
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ShowSegmentChoices();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void ShowSegmentChoices()
    {
        List<TowerSegment> possibleChoices = new List<TowerSegment>(availableSegments);
        _chosenSegments = new TowerSegment[3];
        menuOverlay.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            if (possibleChoices.Count == 0)
                break; // If we run out of unique segments, stop selecting
            
            TowerSegment chosenSegment = GetValidSegment(possibleChoices);
            if (chosenSegment != null)
            {
                _chosenSegments[i] = chosenSegment;
                possibleChoices.Remove(chosenSegment); // Prevent duplicates in this round
            }

            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _chosenSegments[i].name;

            int index = i; // Capture index for lambda expression
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => SelectSegment(index));
        }
    }
    
    public void SelectSegment(int index)
    {
        TowerSegment newSegment = Instantiate(_chosenSegments[index]);
        towerManager.AddSegment(newSegment);

        menuOverlay.SetActive(false);
        foreach (Button button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }
    
    private TowerSegment GetValidSegment(List<TowerSegment> choices)
    {
        TowerSegment chosen = null;
        int attempts = 10; // Prevent infinite loops

        while (attempts > 0 && choices.Count > 0)
        {
            int randomIndex = Random.Range(0, choices.Count);
            TowerSegment segment = choices[randomIndex];
            chosen = segment;
            attempts--;
        }

        return chosen;
    }
}
