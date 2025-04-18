using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float moveUpAmount = 5f;
    [SerializeField]
    float acceptableSnapDistance = 0.05f;
    [SerializeField]
    float snapTime = .5f;
    [SerializeField]
    float acceptableZoomDistance = 0.05f;
    [SerializeField]
    float zoomTime = 1f;
    [SerializeField]
    float[] zoomLevels;
    int currentZoomIndex = 0;
    private float targetZoom;
    public float cameraSpeed;
    private Camera _zoomCamera;

    private Vector3 targetPosition;
    private int zoomAnimationFrames;
    
    int elapsedZoomFrames = 0;
    private int snapAnimationFrames;
    int elapsedSnapFrames = 0;
    bool keyHeld = false;
    public bool atPosition = true;
    public bool blockUpInput = false;
    public bool blockDownInput = false;
    public bool blockLeftInput = false;
    public bool blockRightInput = false;

    private void Awake()
    {
        zoomAnimationFrames = (int) (zoomTime / Time.fixedDeltaTime); 
        snapAnimationFrames = (int) (snapTime / Time.fixedDeltaTime); 
        _zoomCamera = Camera.main;
        _zoomCamera.orthographicSize = zoomLevels[currentZoomIndex];
        targetZoom = zoomLevels[currentZoomIndex];
        targetPosition = gameObject.transform.position;
        // Transform testTransform = gameObject.transform;
        // testTransform.position = new Vector3(targetPosition.x, targetPosition.y + 10, targetPosition.z);
        // CameraFocus(testTransform);
    }
    
    protected virtual void Update()
    {
        
        if (Input.GetKey(KeyCode.W) && !blockUpInput)
        {
            CameraMoveUp();
            keyHeld = true;
            // targetPosition.y += cameraSpeed * Time.deltaTime;
            // gameObject.transform.position = targetPosition;
        }
        if (Input.GetKey(KeyCode.S) && !blockDownInput)
        {
            CameraMoveDown();
            keyHeld = true;
            // targetPosition.y -= cameraSpeed * Time.deltaTime;
            // gameObject.transform.position = targetPosition;
        }
        
        
        if (Input.GetKey(KeyCode.A) && !blockLeftInput)
        {
            keyHeld = true;
            CameraMoveLeft();
        }

        if (Input.GetKey(KeyCode.D) && !blockRightInput)
        {
            keyHeld = true;
            CameraMoveRight();
        }
        

        //If absolutely no keys are down set the elapsed frames to zero
        if ((Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S)) && (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)))
        {
            elapsedSnapFrames = 0;
            keyHeld = false;
        } else if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
        {
            elapsedSnapFrames = 0;
            keyHeld = false;
        }

        if (atPosition)
        {
            if (!Input.GetKey(KeyCode.W))
                blockUpInput = false;
            if (!Input.GetKey(KeyCode.S))
                blockDownInput = false;
            if (!Input.GetKey(KeyCode.A))
                blockLeftInput = false;
            if (!Input.GetKey(KeyCode.D))
                blockRightInput = false;

        }


        // Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        // if (Input.GetAxis("Mouse ScrollWheel") > 0)
        //     SetZoomLevel(currentZoomIndex - 1);
        // else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        //     SetZoomLevel(currentZoomIndex + 1);

        if (Input.GetKeyDown(KeyCode.T)){
            // Debug.Break();
            SetZoomLevel(currentZoomIndex - 1);
        }else if (Input.GetKeyDown(KeyCode.G)){
            // Debug.Break();
            SetZoomLevel(currentZoomIndex + 1);
        }



    }


    void FixedUpdate()
    {


        

        // Position Lerp
        if (Vector3.Distance(targetPosition, transform.position) > acceptableSnapDistance){

            float interpolationRatio = (float)elapsedSnapFrames / (float)snapAnimationFrames;
            transform.position = Vector3.Lerp(transform.position, targetPosition, interpolationRatio);
            elapsedSnapFrames = (elapsedSnapFrames + 1) % (snapAnimationFrames + 1);
            elapsedSnapFrames = !keyHeld ? (elapsedSnapFrames + 1) % (snapAnimationFrames + 1) : snapAnimationFrames / 2;
            Debug.Log("" + elapsedSnapFrames);

        } else {
            transform.position = targetPosition;//snap to the targetPos
            atPosition = true;
            
        }

        //Zoom Lerp
        if (Mathf.Abs(_zoomCamera.orthographicSize - targetZoom) > acceptableZoomDistance){
            Debug.Log("Camera Zoom: " + _zoomCamera.orthographicSize);
            float interpolationRatio = (float)elapsedZoomFrames / (float)zoomAnimationFrames;
            _zoomCamera.orthographicSize = Mathf.Lerp(_zoomCamera.orthographicSize, targetZoom, interpolationRatio);
            elapsedZoomFrames = (elapsedZoomFrames + 1) % (zoomAnimationFrames + 1);

        } else {
            _zoomCamera.orthographicSize = targetZoom;//snap to the targetPos

        }
    }

    public void CameraFocus(Transform transform, bool zoomIn = false, float zoom = 5){
        elapsedSnapFrames = 0;
        targetPosition.x = transform.position.x;
        targetPosition.y = transform.position.y;
        if (zoomIn){
            SetZoom(zoom);
        }
    }

    public void CameraControl(Transform transform){
        elapsedSnapFrames = 0;
        targetPosition.x = transform.position.x;
        targetPosition.y = transform.position.y;
        atPosition = false;
        blockLeftInput = true;
        blockRightInput = true;
        blockUpInput = true;
        blockDownInput = true;
    }

    public void CameraMoveUp(){
        // elapsedSnapFrames = 0;
        float moveFactor = zoomLevels[currentZoomIndex] / zoomLevels[0];
        // targetPosition.y += moveUpAmount * Time.deltaTime * moveFactor;

        
        targetPosition.y += cameraSpeed * Time.deltaTime * moveFactor;
        // gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, .05f);

    }

    public void CameraMoveDown(){
        // elapsedSnapFrames = 0;
        float moveFactor = zoomLevels[currentZoomIndex] / zoomLevels[0];
        // targetPosition.y -= moveUpAmount * Time.deltaTime * moveFactor;

        targetPosition.y -= cameraSpeed * Time.deltaTime * moveFactor;
        // gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, .05f);
    }

    public void CameraMoveRight()
    {

        float moveFactor = zoomLevels[currentZoomIndex] / zoomLevels[0];
        targetPosition.x += cameraSpeed * Time.deltaTime * moveFactor;
    }

    public void CameraMoveLeft()
    {
        float moveFactor = zoomLevels[currentZoomIndex] / zoomLevels[0];
        targetPosition.x -= cameraSpeed * Time.deltaTime * moveFactor;
    }
    public void CameraUpLock()
    {
        blockUpInput = true;
    }
    public void CameraDownLock()
    {
        blockDownInput = true;
    }
    public void CameraLeftLock()
    {
        blockLeftInput = true;
    }
    public void CameraRightLock()
    {
        blockRightInput = true;
    }

    public void SetZoomLevel(int zoomLevel){
        elapsedZoomFrames = 0;
        Debug.Log("Raw input: " + zoomLevel);
        zoomLevel = Math.Clamp(zoomLevel, 0, zoomLevels.Length -1);
        Debug.Log("Setting Zoom to level " + zoomLevel);
        targetZoom = zoomLevels[zoomLevel];
        currentZoomIndex = zoomLevel;
    }

    public void SetZoom(float zoom){
        targetZoom = zoom;
    }

    internal void ImmediateSetZoom(float cameraZoom)
    {
        _zoomCamera.orthographicSize = cameraZoom;
        targetZoom = cameraZoom;
    }
    public float GetTargetZoom(){
        return targetZoom;
    }
}
