using UnityEngine;
using UnityEngine.Audio;

public class MiniWave : MonoBehaviour
{
    [SerializeField]
    public AudioResource miniWaveStart;
    [SerializeField]
    public AudioResource miniWaveOver;
    bool childrenAdded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MusicManager.instance.PlayWaveMusic(miniWaveStart as AudioClip, true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.childCount > 0)
            childrenAdded = true;//just incase fixed update is called before we get to add the children
        if(transform.childCount <= 0 && childrenAdded){
        MusicManager.instance.PlayWaveMusic(miniWaveOver as AudioClip, true);
            Destroy(gameObject);
        }
    }
}
