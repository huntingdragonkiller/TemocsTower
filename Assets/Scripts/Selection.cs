using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField]
    Image selectionImage;
    [SerializeField]
    TextMeshProUGUI selectionName;
    [SerializeField]
    TextMeshProUGUI selectionDescription;

    SelectionScriptableObject currentSelection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Chosen(){
        Debug.Log("They chose me, yay!!!");
        FindAnyObjectByType<SelectionManager>().HandleChosenSelection(currentSelection);
    }

    //Returns false if the passed selection is the same as the current selection
    //If they aren't changes the display and returns true
    //This helps avoid duplicate selections from previous round, at least on the same panel
    public bool ChangeSelection(SelectionScriptableObject toChange)
    {
        
        DisplaySelection(toChange);
        return true;
    }

    private void DisplaySelection(SelectionScriptableObject selection)
    {
        currentSelection = selection;
        selectionImage.sprite = currentSelection.Sprite;
        selectionName.text = currentSelection.Name;
        selectionDescription.text = currentSelection.Description;
    }

}
