using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    AudioClip normalMusic;
    [SerializeField]
    AudioClip bossMusic;
    [SerializeField]
    AudioClip menuMusic;
    [SerializeField]
    AudioClip defeatMusic;
    [SerializeField]
    AudioClip victoryMusic;
    [SerializeField]
    float transitionTime = 1.5f;
    [SerializeField]
    AudioSource musicSoundObject;
    int animationFrames;
    int elapsedFrames = 0;

    AudioSource currentMusic;
    AudioSource toSwap;
    IEnumerator swapRoutine;
    public static MusicManager instance;
    bool loadin = true;
    bool canSwap = true;

    void Awake()
    {
        if (instance == null){
            instance = this;
        }
        if (loadin){
            currentMusic = Instantiate(musicSoundObject, transform);
            currentMusic.clip = menuMusic;
            currentMusic.Play();
            // animationFrames = (int) (transitionTime / Time.fixedDeltaTime);
            loadin  =   false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMenuMusic(bool waitTillDone = false){
        if(swapRoutine != null)
            StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(menuMusic, waitTillDone);
        StartCoroutine(swapRoutine);
    }
    public void PlayNormalMusic(bool waitTillDone = false){
        if(swapRoutine != null)
            StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(normalMusic, waitTillDone);
        StartCoroutine(swapRoutine);
    }
    public void PlayBossMusic(bool waitTillDone = false){
        if(swapRoutine != null)
            StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(bossMusic, waitTillDone);
        StartCoroutine(swapRoutine);
    }
    public void PlayWaveMusic(AudioClip song, bool waitTillDone = false){
        if(swapRoutine != null)
            StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(song, waitTillDone);
        StartCoroutine(swapRoutine);
    }

    //Slowly fades out the current music, and fades in the new music
    private IEnumerator SwapToMusic(AudioClip swappingTo, bool waitTillDone = false)
    {
        if(canSwap){
            currentMusic.Stop();
            currentMusic.clip = swappingTo;
            currentMusic.Play();
            yield return null; 
        } else {
            while(currentMusic.isPlaying){yield return new WaitForFixedUpdate();}
            currentMusic.clip = swappingTo;
            canSwap = !waitTillDone;
            currentMusic.Play();
        }
        // elapsedFrames = 0;
        // if(toSwap == null){
        //     toSwap = Instantiate(musicSoundObject, transform);
        // }
        // toSwap.clip = swappingTo;
        // toSwap.loop = looping;
        // toSwap.volume = 0f;
        // toSwap.Play();
        // while (elapsedFrames <= animationFrames){
        //     float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
        //     toSwap.volume = Mathf.Lerp(toSwap.volume, 1f, interpolationRatio);
        //     currentMusic.volume = Mathf.Lerp(currentMusic.volume, 0f, interpolationRatio);
        //     elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
        //     yield return new WaitForFixedUpdate();
        // }
        // toSwap.volume = 1f;
        // Destroy(currentMusic);
        // currentMusic = toSwap;
        // toSwap = null;
    }
}
