﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AudioData
{
    public AudioClip clip;
    public float volume = 1f;
}

/// <summary>
/// BGMとSEの管理をするマネージャ。シングルトン。
/// </summary>
[System.Serializable]
public class AudioManager : Singleton<AudioManager>
{
    [Header("BGM")]
    public AudioData BGM_STAGE1;
    public AudioData BGM_STAGE2;
    public AudioData BGM_STAGE3;
    public AudioData BGM_STAGE4;
    public AudioData BGM_TITLE;
    public AudioData BGM_END;

    [Header("足音")]
    public AudioData SE_FOOTSTEP_GRASS;
    public AudioData SE_FOOTSTEP_ROCK;
    public AudioData SE_FOOTSTEP_SAND;
    public AudioData SE_FOOTSTEP_SNOW1;
    public AudioData SE_FOOTSTEP_SNOW2;

    [Header("軸系")]
    public AudioData SE_PLANETROTATION;
    public AudioData SE_DEPLOY;
    public AudioData SE_FLAGPLUGIN;
    public AudioData SE_FLAGUNPLUG;
    public AudioData SE_FLOATSWAP;
    public AudioData SE_FLOATGROUNDENTER;

    [Header("アイテム系")]
    public AudioData SE_GETSTAR1;
    public AudioData SE_GETSTAR2;
    public AudioData SE_COMPLETESTAR;
    public AudioData SE_GETDIAMOND1;
    public AudioData SE_GETDIAMOND2;

    [Header("衝撃音")]
    public AudioData SE_IMPACT_GRASS;
    public AudioData SE_IMPACT_ROCK;
    public AudioData SE_IMPACT_SAND;
    public AudioData SE_IMPACT_SNOW;

    [Header("ロケット")]
    public AudioData SE_FANFARE;
    public AudioData SE_ROCKET;
    public AudioData SE_METAL;

    [Header("選択系")]
    public AudioData SE_SUCCESS;
    public AudioData SE_SELECT;
    public AudioData SE_RETURN;

    //オーディオファイルのパス
    private const string BGM_PATH = "Audio/BGM";
    private const string SE_PATH = "Audio/SE";

    //ボリューム保存用のkeyとデフォルト値
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const float BGM_VOLUME_DEFULT = 1.0f;

    //BGMがフェードするのにかかる時間
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 1.5f;
    private float _bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    //次流すBGM名、SE名
    private AudioData _nextBGM = new AudioData();

    //BGMをフェードアウト中か
    private bool _isFadeOut = false;

    //オーディオソースを持つ
    private AudioSource _bgmSource;
    private const int BGM_SOURCE_NUM = 1;

    private float BGM_curVolume = 0.0f;
    public float BGM_masterVolume;
    public float SE_masterVolume;

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        //オーディオリスナーおよびオーディオソースをSE+BGM分作成
        gameObject.AddComponent<AudioListener>();
        for (int i = 0; i < BGM_SOURCE_NUM; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }

        //作成したオーディオソースを取得して各変数に設定、ボリュームも設定
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();

        for (int i = 0; i < audioSourceArray.Length; i++)
        {
            audioSourceArray[i].playOnAwake = false;

            //BGMの設定を初期化
            if (i < BGM_SOURCE_NUM)
            {
                audioSourceArray[i].loop = true;
                _bgmSource = audioSourceArray[i];
                _bgmSource.Stop();
                _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            }
        }

    }
    //=================================================================================
    //BGM
    //=================================================================================

    /// <summary>
    /// 指定したファイル名のBGMを流す。ただし既に流れている場合は前の曲をフェードアウトさせてから。
    /// 第二引数のfadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void PlayBGM(AudioData bgm, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!bgm.clip)
        {
            Debug.Log("BGMがありません");
            return;
        }

        BGM_curVolume = bgm.volume;

        //現在BGMが流れていない時はそのまま流す
        if (!_bgmSource.isPlaying)
        {
            DataManager data = DataManager.Instance;
            BGM_masterVolume = (data.commonData.BGM_Volume / 1000);
            _nextBGM.clip = null;
            _nextBGM.volume = 0;
            _bgmSource.clip = bgm.clip;
            _bgmSource.volume = BGM_curVolume * BGM_masterVolume;
            _bgmSource.Play();
        }
        //違うBGMが流れている時は、流れているBGMをフェードアウトさせてから次を流す。同じBGMが流れている時はスルー
        else if (_bgmSource.clip != bgm.clip)
        {
            _nextBGM = bgm;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    public void PlaySEOneShot(AudioSource sorce, AudioData audioData)
    {
        if (!audioData.clip)
        {
            Debug.Log("SEがありません");
            return;
        }
       
        sorce.PlayOneShot(audioData.clip, audioData.volume * SE_masterVolume);
    }

    public void PlaySE(AudioSource sorce, AudioData audioData)
    {
        if (!audioData.clip)
        {
            Debug.Log("SEがありません");
            return;
        }

        sorce.clip = audioData.clip;
        sorce.volume = audioData.volume * SE_masterVolume;
        sorce.Play();
    }

    public void StopSE(AudioSource sorce)
    {
        sorce.Stop();
    }

    /// <summary>
    /// BGMをすぐに止める
    /// </summary>
    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    /// <summary>
    /// 現在流れている曲をフェードアウトさせる
    /// fadeSpeedRateに指定した割合でフェードアウトするスピードが変わる
    /// </summary>
    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        _bgmFadeSpeedRate = fadeSpeedRate;
        _isFadeOut = true;
    }

    private void Update()
    {
        DataManager data = DataManager.Instance;
        BGM_masterVolume = (data.commonData.BGM_Volume / 1000);
        SE_masterVolume = data.commonData.SE_Volume / 100;
        

        if (!_isFadeOut)
        {
            _bgmSource.volume = BGM_curVolume * BGM_masterVolume;
            return;
        }

        //徐々にボリュームを下げていき、ボリュームが0になったらボリュームを戻し次の曲を流す
        _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
        if (_bgmSource.volume <= 0)
        {
            _bgmSource.Stop();
            _bgmSource.volume = 0f;
            _isFadeOut = false;

            if (!_nextBGM.clip)
            {
                PlayBGM(_nextBGM);
            }
        }

    }

    //=================================================================================
    //音量変更
    //=================================================================================

    /// <summary>
    /// BGMとSEのボリュームを別々に変更&保存
    /// </summary>
    public void ChangeVolume(float BGMVolume)
    {
        _bgmSource.volume = BGMVolume;

        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
    }


    public void ChangeSEVolume(AudioSource audioSource, AudioData audioData, float volumePercent)
    {
        audioSource.volume = audioData.volume * SE_masterVolume * volumePercent;
    }
}