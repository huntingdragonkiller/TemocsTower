using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    private Camera _zoomCamera;
    private Vector3 _elevation;

    private void Start()
    {
        _zoomCamera = Camera.main;
        _elevation = gameObject.transform.position;
    }
    
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            _elevation.y += cameraSpeed * Time.deltaTime;
            gameObject.transform.position = _elevation;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _elevation.y -= cameraSpeed * Time.deltaTime;
            gameObject.transform.position = _elevation;
        }
            

        
        
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            _zoomCamera.orthographicSize -= 1;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            _zoomCamera.orthographicSize += 1;
    }
}
