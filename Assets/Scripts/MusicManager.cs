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

    void Awake()
    {
        currentMusic = Instantiate(musicSoundObject, transform);
        currentMusic.clip = menuMusic;
        currentMusic.Play();
        animationFrames = (int) (transitionTime / Time.fixedDeltaTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayMenuMusic(){
        StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(menuMusic);
        StartCoroutine(swapRoutine);
    }
    public void PlayNormalMusic(){
        StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(normalMusic);
        StartCoroutine(swapRoutine);
    }
    public void PlayBossMusic(){
        StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(bossMusic);
        StartCoroutine(swapRoutine);
    }
    public void PlayDefeatMusic(){
        StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(defeatMusic);
        StartCoroutine(swapRoutine);
    }
    public void PlayVictoryMusic(){
        StopCoroutine(swapRoutine);
        swapRoutine = SwapToMusic(victoryMusic);
        StartCoroutine(swapRoutine);
    }

    //Slowly fades out the current music, and fades in the new music
    private IEnumerator SwapToMusic(AudioClip swappingTo, bool looping = true)
    {
        elapsedFrames = 0;
        if(toSwap == null){
            toSwap = Instantiate(musicSoundObject, transform);
        }
        toSwap.clip = swappingTo;
        toSwap.loop = looping;
        toSwap.volume = 0f;
        toSwap.Play();
        while (elapsedFrames <= animationFrames){
            float interpolationRatio = (float)elapsedFrames / (float)animationFrames;
            toSwap.volume = Mathf.Lerp(toSwap.volume, 1f, interpolationRatio);
            currentMusic.volume = Mathf.Lerp(currentMusic.volume, 0f, interpolationRatio);
            elapsedFrames = (elapsedFrames + 1) % (animationFrames + 1);
            yield return new WaitForFixedUpdate();
        }
        toSwap.volume = 1f;
        Destroy(currentMusic);
        currentMusic = toSwap;
        toSwap = null;
    }
}
