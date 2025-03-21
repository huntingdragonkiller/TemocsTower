using System;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject overlay;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject settingsMenu;
    bool paused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!paused)
                OpenPauseMenu();
            else{
                ClosePauseMenu();
            }
        }
    }


    public void OpenPauseMenu()
    {
        overlay.SetActive(true);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void ClosePauseMenu()
    {

        Time.timeScale = 1.0f;
        paused = false;
        pauseMenu.SetActive(false);
        overlay.SetActive(false);
    }

    public void OpenSettingsMenu(){
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void Quit(){
        Debug.Log("Quitting");
        Application.Quit();
    }
}
