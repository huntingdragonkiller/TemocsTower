using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    GameObject mainSettingsScreen;

    [SerializeField]
    GameObject audioSettingsScreen;
    [SerializeField]
    GameObject backButton;
    [SerializeField]
    GameObject doneButton;

    GameObject currentScreen;
    public bool fromPause;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScreen = mainSettingsScreen;
    }
    void OnDisable()
    {
        FindAnyObjectByType<UIManager>().OpenPauseMenu();
    }
    public void Back(){
        if (currentScreen == mainSettingsScreen)
        {
            Close();
        } else {
            currentScreen.SetActive(false);
            mainSettingsScreen.SetActive(true);
            currentScreen = mainSettingsScreen;
        }
    }
    public void Close(){
        mainSettingsScreen.SetActive(false);
        audioSettingsScreen.SetActive(false);
        backButton.SetActive(false);
        doneButton.SetActive(false);
    }

    public void Open(bool fromPause)
    {
        this.fromPause = fromPause;
        currentScreen = mainSettingsScreen;
        currentScreen.SetActive(true);
        backButton.SetActive(true);
        doneButton.SetActive(true);
    }

    public void OpenAudioSettings(){
        audioSettingsScreen.SetActive(true);
        mainSettingsScreen.SetActive(false);
        currentScreen = audioSettingsScreen;
    }

}
