using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    List<SelectionButton> selections = new List<SelectionButton>();
    [SerializeField]
    public List<SelectionScriptableObject> availableSelections; // Pool of possible choices for selections

    List<SelectionScriptableObject> currentSelections;
    public TowerManager towerManager; // Reference to the tower
    bool shouldBePaused = false;
     


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            NewSelections();
            // ShowSegmentChoices();
        if(shouldBePaused && Time.timeScale != 0)
        {
            //Repause
            FindAnyObjectByType<UIManager>().Pause();
        }
    }

    public void NewSelections(){

        FindAnyObjectByType<UIManager>().Pause();
        shouldBePaused = true;
        foreach (SelectionButton s in selections){
            s.gameObject.SetActive(true);
        }
        List<SelectionScriptableObject> toDisplay = RerollSelections();

        for (int i = 0; i < toDisplay.Count; i++){
            selections[i].ChangeSelection(toDisplay[i]);
        }

    }

    private List<SelectionScriptableObject> RerollSelections()
    {
        List<SelectionScriptableObject> currentRolls = new List<SelectionScriptableObject>(availableSelections);
        
        //Trims the results down to the size of possible selections by randomly removing one at a time
        while (currentRolls.Count > selections.Count){
            int index = Random.Range(0, currentRolls.Count);
            currentRolls.RemoveAt(index);
        }
        currentSelections = currentRolls;
        return currentSelections;
    }

    public void HandleChosenSelection(SelectionScriptableObject chosen){
        shouldBePaused = false;
        GameObject chosenObject = chosen.Selection;
        TowerSegment segment = chosenObject.GetComponent<TowerSegment>();
        if (segment != null){
            towerManager.AddSegment(segment);
        }

        FindAnyObjectByType<UIManager>().Unpause();
        foreach (SelectionButton s in selections){
            s.gameObject.SetActive(false);
        }

        FindFirstObjectByType<ShopManager>().AddNewSelection(chosen);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    // public void ShowSegmentChoices()
    // {
    //     List<TowerSegment> possibleChoices = new List<TowerSegment>(availableSegments);
    //     _chosenSegments = new TowerSegment[3];
    //     menuOverlay.SetActive(true);

    //     for (int i = 0; i < 3; i++)
    //     {
    //         if (possibleChoices.Count == 0)
    //             break; // If we run out of unique segments, stop selecting
            
    //         TowerSegment chosenSegment = GetValidSegment(possibleChoices);
    //         if (chosenSegment != null)
    //         {
    //             _chosenSegments[i] = chosenSegment;
    //             possibleChoices.Remove(chosenSegment); // Prevent duplicates in this round
    //         }

    //         optionButtons[i].gameObject.SetActive(true);
    //         optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = _chosenSegments[i].name;

    //         int index = i; // Capture index for lambda expression
    //         optionButtons[i].onClick.RemoveAllListeners();
    //         optionButtons[i].onClick.AddListener(() => SelectSegment(index));
    //     }
    // }
    
    // public void SelectSegment(int index)
    // {
    //     TowerSegment newSegment = _chosenSegments[index];
    //     towerManager.AddSegment(newSegment);

    //     menuOverlay.SetActive(false);
    //     foreach (Button button in optionButtons)
    //     {
    //         button.gameObject.SetActive(false);
    //     }
    //     FindAnyObjectByType<ShopManager>().InitializeShop();
    // }
    
    // private TowerSegment GetValidSegment(List<TowerSegment> choices)
    // {
    //     TowerSegment chosen = null;
    //     int attempts = 10; // Prevent infinite loops

    //     while (attempts > 0 && choices.Count > 0)
    //     {
    //         int randomIndex = Random.Range(0, choices.Count);
    //         TowerSegment segment = choices[randomIndex];
    //         chosen = segment;
    //         attempts--;
    //     }

    //     return chosen;
    // }
}
