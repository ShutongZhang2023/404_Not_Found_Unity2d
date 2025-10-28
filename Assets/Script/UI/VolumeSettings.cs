using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Mixer & Exposed Params")]
    public AudioMixer mixer;
    public string masterParam = "MasterVolume";
    public string bgmParam = "BgmVolume";
    public string sfxParam = "SfxVolume";

    [Header("UI")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    const float MIN_DB = -80f;
    const float MIN_T = 0.0001f;

    void Start()
    {
        // 从存档读回（没存过就给默认 1）
        masterSlider.value = PlayerPrefs.GetFloat("vol_master", 1f);
        bgmSlider.value = PlayerPrefs.GetFloat("vol_bgm", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("vol_sfx", 1f);
        ApplyAll();
    }

    public void OnMasterChanged(float v)
    {
        SetDb(masterParam, v);
        PlayerPrefs.SetFloat("vol_master", v);
    }

    public void OnBgmChanged(float v)
    {
        SetDb(bgmParam, v);
        PlayerPrefs.SetFloat("vol_bgm", v);
    }

    public void OnSfxChanged(float v)
    {
        SetDb(sfxParam, v);
        PlayerPrefs.SetFloat("vol_sfx", v);
    }

    void ApplyAll()
    {
        SetDb(masterParam, masterSlider.value);
        SetDb(bgmParam, bgmSlider.value);
        SetDb(sfxParam, sfxSlider.value);
    }

    void SetDb(string param, float t01)
    {
        float t = Mathf.Clamp(t01, MIN_T, 1f);
        float dB = Mathf.Lerp(MIN_DB, 0f, Mathf.Log10(t) / Mathf.Log10(1f));
        dB = 20f * Mathf.Log10(t);   
        dB = Mathf.Max(dB, MIN_DB);
        mixer.SetFloat(param, dB);
    }
}
