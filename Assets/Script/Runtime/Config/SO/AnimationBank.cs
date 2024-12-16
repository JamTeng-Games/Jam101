using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using Quantum.Graph.Skill;
using UnityEngine;

// [CreateAssetMenu(fileName = "AnimationCollection", menuName = "Jam/AnimationCollection")]
// public class AnimationCollection : ScriptableObject
// {
//     public TransitionAsset attack1;
//     public TransitionAsset attack2;
//     public TransitionAsset attack3;
//     public TransitionAsset attack4;
//
//     public TransitionAsset defend;
//     public TransitionAsset defendHit;
//
//     public TransitionAsset dieStart;
//     public TransitionAsset dieStay;
//
//     public TransitionAsset getHit;
//
//     public TransitionAsset idle;
//     public TransitionAsset idleBattle;
//
//     public TransitionAsset moveForward;
//     public TransitionAsset moveBackward;
//
//     public TransitionAssetBase moveMixerNormal;
//     public TransitionAssetBase moveMixerBattle;
//
//     public TransitionAsset victory;
//
//     // Parameters
//     public StringAsset pMoveSpeed;
// }

[CreateAssetMenu(fileName = "AnimationBank", menuName = "Jam/AnimationBank")]
public class AnimationBank : ScriptableObject
{
    public List<AnimationInfo> animations;

    // Parameters
    public StringAsset pMoveSpeed;

    public AnimationInfo GetAnimation(AnimationKey key)
    {
        if (animations == null)
            return AnimationInfo.InValid;

        foreach (var anim in animations)
        {
            if (anim.key == key)
                return anim;
        }
        return AnimationInfo.InValid;
    }
}

[Serializable]
public struct AnimationInfo
{
    public AnimationKey key;
    public ClipTransition animation;
    public int priority;
    public bool canReplay;

    public static AnimationInfo InValid = new AnimationInfo()
    {
        key = AnimationKey.None, animation = null, priority = 0, canReplay = false
    };

    public bool IsValid()
    {
        return key != AnimationKey.None;
    }
}