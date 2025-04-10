using System;
using UnityEngine;

public class SummoningCircle : MonoBehaviour
{
    [SerializeField]
    float degreePerSecond = 5f;
    float degreePerFrame;
    private static readonly int Summon = Animator.StringToHash("Summon");
    private static readonly int SummonSpeed = Animator.StringToHash("SummonSpeed");
    float summonAnimationLength;

    protected Animator anim;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach(AnimationClip clip in clips){
            switch(clip.name){
                case "Summoning":
                    summonAnimationLength = clip.length;
                    break;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        degreePerFrame = degreePerSecond * Time.fixedDeltaTime;
        transform.Rotate(Vector3.forward * degreePerFrame, Space.Self);
    }
    
    internal void StartSummon(float SummonTime)
    {
        anim.SetTrigger(Summon);
        anim.SetFloat(SummonSpeed, summonAnimationLength/SummonTime);
    }
}
