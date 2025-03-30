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


    //Not where I want it yet, needs to be smoother with the zoom and not "stick" when it hits the bounds
    private void Update()
    {
        float camHeight = cameraController.GetTargetZoom();// we do this instead of mainCamera.orthographicSize to account for zooming out near the boundaries
        float camWidth = camHeight * mainCamera.aspect;
        if (mainCamera.transform.position.x + camWidth > right)
        {
            cameraController.CameraRightLock();
            //need to put this outside the if?? maybe the problems in the cam controller
            if (!Input.GetKey(KeyCode.D))
                cameraController.CameraMoveLeft();
        }
        else if (mainCamera.transform.position.x - camWidth < left)
        {
            cameraController.CameraLeftLock();
            if (!Input.GetKey(KeyCode.A))
                cameraController.CameraMoveRight();
        }

        if (mainCamera.transform.position.y + camHeight > top)
        {
            cameraController.CameraUpLock();
            if (!Input.GetKey(KeyCode.W))
                cameraController.CameraMoveDown();
        }
        else if (mainCamera.transform.position.y - camHeight < bottom)
        {
            cameraController.CameraDownLock();
            if (!Input.GetKey(KeyCode.S))
                cameraController.CameraMoveUp();
        }

    }

}
