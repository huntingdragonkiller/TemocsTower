using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            transform.position += new Vector3(0, cameraSpeed, 0);
        if(Input.GetKey(KeyCode.S))
            transform.position -= new Vector3(0, cameraSpeed, 0);
    }
}
