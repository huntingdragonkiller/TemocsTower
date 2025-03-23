using UnityEngine;

public class WinMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Replay(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Debug.Log("Quit");   
        Application.Quit();
    }
}
