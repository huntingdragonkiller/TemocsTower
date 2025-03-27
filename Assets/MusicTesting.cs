using UnityEngine;

public class MusicTesting : MonoBehaviour
{
    [SerializeField]
    AudioClip miniwaveSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PauseMusic()
    {
        MusicManager.instance.PauseMusic();
    }
    public void PlayMusic()
    {
        MusicManager.instance.UnPause();
    }

    public void SwapMusicOne()
    {
        MusicManager.instance.PlayNormalMusic();
    }

    public void SwapMusicTwo()
    {
        MusicManager.instance.PlayBossMusic();

    }

    public void FadeIn()
    {
        MusicManager.instance.FadeInCurrentMusic(5f, 1f);
    }
    public void FadeOut()
    {
        MusicManager.instance.FadeOutCurrentMusic(5f, 0f);
    }

    public void MiniWave()
    {
        MusicManager.instance.PlayWaveMusic(miniwaveSound);
    }
}
