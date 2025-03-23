using System;
using System.Collections;
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
    [SerializeField]
    SettingsManager settingsManager;
    [SerializeField]
    CameraController mainCamera;
    bool paused = false;
    private MainMenuManager mainMenu;
    private Transform mainMenuTransform;

    void Awake()
    {
        mainMenu = FindAnyObjectByType<MainMenuManager>();
        mainMenuTransform = mainMenu.GetComponent<Transform>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale > 0 )
                OpenPauseMenu();
            else{
                ClosePauseMenu();
            }
        }
    }


    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        if(!paused)
            Pause();
    }

    public void ClosePauseMenu()
    {
        if(paused)
            Unpause();
        pauseMenu.SetActive(false);
    }

    public void Pause(){
        Time.timeScale = 0f;
        paused = true;
        overlay.SetActive(true);
    }

    public void Unpause(){
        Time.timeScale = 1.0f;
        paused = false;
        overlay.SetActive(false);
    }

    public void OpenSettingsMenu(){
        settingsManager.Open(true);
        pauseMenu.SetActive(false);
    }

    public void Quit(){
        Debug.Log("Going back to main menu");
        StartCoroutine(GoBackToMainMenu());
        Application.Quit();
    }

    private IEnumerator GoBackToMainMenu()
    {
        mainCamera.CameraControl(mainMenuTransform);
        mainCamera.SetZoom(mainMenu.cameraStartingZoom);
        Unpause();
        pauseMenu.SetActive(false);
        while(!mainCamera.atPosition){yield return new WaitForFixedUpdate();}
        mainMenu.gameObject.SetActive(true);
    }
}
