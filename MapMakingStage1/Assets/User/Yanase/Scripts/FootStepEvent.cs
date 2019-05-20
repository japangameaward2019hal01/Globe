﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class FootStepEvent : MonoBehaviour {

    public StateManager stateManager = null;
    public PlanetWalker planetWalker = null;
    AudioSource footAudio;

    private void Start()
    {
        footAudio = GetComponent<AudioSource>();
        if (!footAudio) footAudio = gameObject.AddComponent<AudioSource>();
    }

    void FootStep1()
    {
        if (!stateManager.onGround) return;
        if (planetWalker.moveAmount < 0.1f) return;
        AudioManager audioManager = AudioManager.Instance;

        switch(stateManager.groundType)
        {
            case GroundType.Type.Grass:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_GRASS);
                break;
            case GroundType.Type.Rock:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_ROCK);
                break;
            case GroundType.Type.Snow:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_SNOW1);
                break;
            case GroundType.Type.Sand:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_SAND);
                break;
        }
    }

    void FootStep2()
    {
        if (!stateManager.onGround) return;
        if (planetWalker.moveAmount < 0.1f) return;
        AudioManager audioManager = AudioManager.Instance;

        switch (stateManager.groundType)
        {
            case GroundType.Type.Grass:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_GRASS);
                break;
            case GroundType.Type.Rock:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_ROCK);
                break;
            case GroundType.Type.Snow:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_SNOW2);
                break;
            case GroundType.Type.Sand:
                audioManager.PlaySEOneShot(footAudio, audioManager.SE_FOOTSTEP_SAND);
                break;
        }
    }
}
