using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Scene BGM Clips")]
    public AudioClip level0BGM;
    // public AudioClip level2BGM;

    private Dictionary<string, AudioClip> sceneBGMMap;

    [Header("SFX Clips")]
    public AudioClip typeClip;
    public AudioClip tearClip;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeBGMMap();
            PlayBGMForScene("SampleScene");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeBGMMap()
    {
        sceneBGMMap = new Dictionary<string, AudioClip>
        {
            { "SampleScene", level0BGM },
            // { "Level2", level2BGM },
        };
    }

    public void PlayBGMForScene(string sceneName)
    {
        if (sceneBGMMap.TryGetValue(sceneName, out AudioClip clip))
        {
            if (bgmSource.clip != clip && clip != null)
            {
                bgmSource.clip = clip;
                bgmSource.Play();
            }
        }
    }

    public void PlayTypeSFX()
    {
        sfxSource.PlayOneShot(typeClip);
    }

    public void PlayTearSFX() { 
        sfxSource.PlayOneShot(tearClip);
    }
}
