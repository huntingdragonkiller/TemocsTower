using UnityEngine;

public class ManaGenRotater : MonoBehaviour
{
    [SerializeField]
    float degreePerSecond = 30f;
    float degreePerFrame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        degreePerFrame = degreePerSecond * Time.fixedDeltaTime;
        transform.Rotate(Vector3.forward * degreePerFrame, Space.Self);
    }
}
