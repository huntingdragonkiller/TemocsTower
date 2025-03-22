using System;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    GameObject mainSettingsScreen;

    [SerializeField]
    GameObject audioSettingsScreen;

    GameObject currentScreen;
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
        gameObject.SetActive(false);
    }

    public void Open()
    {
        currentScreen = mainSettingsScreen;
        currentScreen.SetActive(true);
    }
    public void OpenAudioSettings(){
        audioSettingsScreen.SetActive(true);
        mainSettingsScreen.SetActive(false);
        currentScreen = audioSettingsScreen;
    }

}
