using UnityEngine;

public class WinDetector : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Tower"){
            
            
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }
}
