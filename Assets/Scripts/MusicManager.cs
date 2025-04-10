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
    IEnumerator fadeInRoutine;
    bool fadingIn = false;
    IEnumerator fadeOutRoutine;
    bool fadingOut = false;
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
        StartCoroutine(Interrupt(song));
    }

    //Slowly fades out the current music, and fades in the new music
    private IEnumerator SwapToMusic(AudioClip swappingTo, bool waitTillDone = false)
    {
        if(canSwap){
            //currentMusic.Stop();
            //currentMusic.clip = swappingTo;
            //currentMusic.Play();
            //yield return null;
            FadeOutCurrentMusic(transitionTime, .2f);
            if (toSwap == null)
                toSwap = Instantiate(musicSoundObject, transform);

            toSwap.clip = swappingTo;
            toSwap.loop = true;
            toSwap.volume = 0f;
            toSwap.Play();

            if (fadeInRoutine != null)
                StopCoroutine(fadeInRoutine);
            fadeInRoutine = FadeIn(toSwap, transitionTime, 1f);
            StartCoroutine(fadeInRoutine);

            while(fadingIn && fadingOut) { yield return new WaitForFixedUpdate(); }

            Destroy(currentMusic.gameObject);
            currentMusic = toSwap;
            toSwap = null;



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

    private IEnumerator Interrupt(AudioClip toPlay)
    {
        AudioSource interrupter = Instantiate(musicSoundObject);

        interrupter.clip = toPlay;
        currentMusic.Pause();
        if(toSwap != null)
            toSwap.Pause();
        interrupter.loop = false;
        interrupter.Play();
        while (interrupter.isPlaying)
        {
            Debug.Log("Waiting");
            yield return new WaitForFixedUpdate();
        }
        Destroy(interrupter.gameObject);
        currentMusic.Play();
        if (toSwap != null)
            toSwap.Play();
    }

    private IEnumerator FadeOut(AudioSource toFade, float duration, float threshold = 0f)
    {
        fadingOut = true;
        float currentTime = 0;
        float start = toFade.volume;
        while (currentTime < duration)
        {

            currentTime += Time.deltaTime;
            //toFade.volume -= start * Time.deltaTime / duration;
            toFade.volume = Mathf.Lerp(start, threshold, currentTime / duration);
            yield return new WaitForFixedUpdate();
        }
        toFade.volume = 0f;
        fadingOut = false;
        yield break;

        //float percentPerFrame = (1f * duration) * Time.deltaTime;
        //while (toFade.volume > threshold) {
        //    toFade.volume -= percentPerFrame;
        //    yield return new WaitForFixedUpdate();
        //}


    }
    private IEnumerator FadeIn(AudioSource toFade, float duration, float threshold = 0f)
    {
        fadingIn = true;
        float currentTime = 0;
        toFade.volume = .2f;
        float start = toFade.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            //toFade.volume += start * (Time.deltaTime / duration);
            toFade.volume = Mathf.Lerp(start, threshold, currentTime / duration);
            yield return new WaitForFixedUpdate();
        }

        toFade.volume = 1f;
        fadingIn = false;
        yield break;
        //float percentPerFrame = (1f * duration) * Time.deltaTime;
        //while (toFade.volume < threshold)
        //{
        //    toFade.volume += percentPerFrame;
        //    yield return new WaitForFixedUpdate();
        //}

    }

    public void FadeOutCurrentMusic(float duration, float threshold)
    {
        if (fadeOutRoutine != null)
            StopCoroutine(fadeOutRoutine);
        fadeOutRoutine = FadeOut(currentMusic, duration, threshold);
        StartCoroutine(fadeOutRoutine);
    }
    public void FadeInCurrentMusic(float duration, float threshold)
    {
        if (fadeInRoutine != null)
            StopCoroutine(fadeInRoutine);
        fadeInRoutine = FadeIn(currentMusic, duration, threshold);
        StartCoroutine(fadeInRoutine);
    }

    public void PauseMusic()
    {
        currentMusic.Pause();
    }
    public void UnPause()
    {
        currentMusic.Play();
    }

}
