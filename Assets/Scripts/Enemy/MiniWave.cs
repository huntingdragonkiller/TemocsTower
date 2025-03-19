using UnityEngine;

public class MiniWave : MonoBehaviour
{
    bool childrenAdded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.childCount > 0)
            childrenAdded = true;//just incase fixed update is called before we get to add the children
        if(transform.childCount <= 0 && childrenAdded){
            Destroy(gameObject);
        }
    }
}
