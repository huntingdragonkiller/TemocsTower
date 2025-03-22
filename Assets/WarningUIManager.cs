using UnityEngine;
using UnityEngine.Audio;

public class WarningUIManager : MonoBehaviour
{
    [SerializeField]
    WarningCamera warningCamera;
    [SerializeField]
    GameObject warningPanel;
    [SerializeField]
    CameraController mainCamera;
    [SerializeField]
    AudioResource warningSound;
    public static WarningUIManager instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
            instance = this; 
    }
    void Start()
    {
        warningCamera.enabled = false;
    }

    public void ShowWarning(GameObject toWarn){
        warningCamera.CameraFocus(toWarn.transform, true);
        SoundFXManager.instance.PlaySoundFXClip(warningSound, transform, 1f);
        warningCamera.enabled = true;
        warningPanel.SetActive(true);
        Debug.Break();
    }

    public void ClearWarning(){
        warningCamera.enabled = false;
        warningPanel.SetActive(false);
    }

    public void MoveToWarning(){
        mainCamera.CameraFocus(warningCamera.transform, false);
        ClearWarning();
    }
}
