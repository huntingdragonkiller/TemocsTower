using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    [SerializeField]
    Transform[] boundaries;

    Camera mainCamera;
    CameraController cameraController;
    float left = float.MaxValue;
    float right = float.MinValue;
    float top = float.MinValue;
    float bottom = float.MaxValue;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<CameraController>();
        foreach(Transform corner in boundaries)
        {
            left = Mathf.Min(left, corner.position.x);
            right = Mathf.Max(right, corner.position.x);
            bottom = Mathf.Min(bottom, corner.position.y);
            top = Mathf.Max(top, corner.position.y);
        }
    }

    private void FixedUpdate()
    {
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;
        if (mainCamera.transform.position.x + camWidth > right)
        {
            cameraController.CameraMoveLeft();
            cameraController.CameraLock();
        }
        else if (mainCamera.transform.position.x - camWidth < left)
        {
            cameraController.CameraMoveRight();
            cameraController.CameraLock();
        }

        if (mainCamera.transform.position.y + camHeight > top)
        {
            cameraController.CameraMoveDown();
            cameraController.CameraLock();
        }
        else if (mainCamera.transform.position.y - camHeight < bottom)
        {
            cameraController.CameraMoveUp();
            cameraController.CameraLock();
        }

    }

}
