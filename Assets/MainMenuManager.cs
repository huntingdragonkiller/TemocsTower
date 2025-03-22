using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    CameraController mainCamera;
    [SerializeField]
    Transform cameraStartingPosition;
    [SerializeField]
    float cameraStartingZoom;
    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    SettingsManager settingsManager;
    public bool inMainMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        mainCamera.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        
        Time.timeScale = 0f;
        inMainMenu = true;
    }

    void Start(){
        mainCamera.ImmediateSetZoom(cameraStartingZoom);
        mainCamera.enabled = false;
    }
    public void OnEnable(){
        inMainMenu = true;
        ReEnable();
        MainMenu.SetActive(true);
    }
    public void ReEnable(){
        // if(mainCamera.enabled)
            // mainCamera.ImmediateSetZoom(cameraStartingZoom);
        mainCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inMainMenu && Time.timeScale > 0f)
        {
            // MainMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void StartGame(){
        mainCamera.enabled=true;
        inMainMenu = false;
        Time.timeScale = 1f;
        MainMenu.SetActive(false);
        mainCamera.CameraFocus(cameraStartingPosition);
        mainCamera.SetZoomLevel(0);
        gameObject.SetActive(false);
    }

    public void OpenSettings(){
        settingsManager.Open();
    }
    public void Quit(){
        Debug.Log("Quitting");
        Application.Quit();
    }
}
