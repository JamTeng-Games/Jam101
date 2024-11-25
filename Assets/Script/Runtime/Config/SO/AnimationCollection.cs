using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCollection", menuName = "Jam/AnimationCollection")]
public class AnimationCollection : ScriptableObject
{
    public TransitionAsset attack1;
    public TransitionAsset attack2;
    public TransitionAsset attack3;
    public TransitionAsset attack4;

    public TransitionAsset defend;
    public TransitionAsset defendHit;

    public TransitionAsset dieStart;
    public TransitionAsset dieStay;

    public TransitionAsset getHit;

    public TransitionAsset idle;
    public TransitionAsset idleBattle;

    public TransitionAsset moveForward;
    public TransitionAsset moveBackward;

    public TransitionAssetBase moveMixerNormal;
    public TransitionAssetBase moveMixerBattle;

    public TransitionAsset victory;

    // Parameters
    public StringAsset pMoveSpeed;
}