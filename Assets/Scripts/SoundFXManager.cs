using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour
{
    [SerializeField] 
    private AudioSource soundFXObject;
    public static SoundFXManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySoundFXClip(AudioResource audioClip, Transform spawnTransform, float volume){
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        audioSource.resource = audioClip;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
