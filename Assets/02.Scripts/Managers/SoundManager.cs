using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer mixer; // BGM과 SFX 볼륨을 조절할 Audio Mixer

    // BGM 사운드 클립들
    public AudioClip casualSuspense;
    public AudioClip casualTheme3Loop;
    public AudioClip tropicalGame;

    // SFX 사운드 클립들
    public AudioClip hover;
    public AudioClip denied;
    public AudioClip unequip;
    public AudioClip buySell;
    public AudioClip hit;
    public AudioClip teleport;
    public AudioClip cannon;
    public AudioClip magic;

    private AudioSource bgmSource; // BGM을 재생할 AudioSource
    private AudioSource sfxSource; // BGM을 재생할 AudioSource


    public Slider bgmSlider; // BGM 볼륨을 조절할 슬라이더
    public Slider sfxSlider; // SFX 볼륨을 조절할 슬라이더

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // AudioSource 컴포넌트 설정
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true; // BGM을 반복 재생하도록 설정

        sfxSource = gameObject.AddComponent<AudioSource>(); // SFX를 재생할 AudioSource 추가
        sfxSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFXVolume")[0]; // SFX용 Audio Mixer 그룹 설정
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Scene이 로드될 때 호출할 함수 지정

        InitializeSliders();
        ApplySavedVolumes();
    }

    void InitializeSliders()
    {
        // 슬라이더 초기화
        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    void ApplySavedVolumes()
    {
        // 저장된 볼륨 적용
        if (bgmSlider != null)
        {
            float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 0.75f);
            bgmSlider.value = savedBGMVolume;
            SetBGMVolume(savedBGMVolume);
        }

        if (sfxSlider != null)
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            sfxSlider.value = savedSFXVolume;
            SetSFXVolume(savedSFXVolume);
        }
    }

    // Scene이 로드될 때 호출되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Scene 이름에 따라 BGM 재생
        switch (scene.name)
        {
            case "IntroScene":
                PlayBGM(casualSuspense);
                break;
            case "Yuyerin":
                PlayBGM(casualTheme3Loop);
                break;
                // 추가적인 Scene이 있으면 여기에 추가할 수 있습니다.
        }

        // 슬라이더 연결 재설정
        InitializeSliders();
        ApplySavedVolumes();
    }

    // BGM 재생
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip == null) return;

        bgmSource.clip = bgmClip;
        bgmSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGMVolume")[0]; // Audio Mixer 설정 적용
        bgmSource.Play();
    }

    // SFX 재생
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip == null) return;

        AudioSource.PlayClipAtPoint(sfxClip, Camera.main.transform.position);
    }

    // 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        if (volume == 0)
        {
            mixer.SetFloat("BGMVolume", -80f); // 볼륨을 최소로 설정 (음소거)
        }
        else
        {
            mixer.SetFloat("BGMVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        Debug.Log("Setting SFX Volume: " + volume);

        if (volume == 0)
        {
            mixer.SetFloat("SFXVolume", -80f); // 볼륨을 최소로 설정 (음소거)
        }
        else
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    // 버튼 클릭 SFX 재생
    public void PlayButtonClickSFX()
    {
        PlaySFX(hover);
    }

    public void PlayEquipSFX()
    {
        PlaySFX(unequip);
    }


    public void PlayFactoryUpgradeSFX()
    {
        PlaySFX(magic);
    }

    public void PlayGoDungeonSFX()
    {
        PlaySFX(teleport);
    }

    // 특정 조건 SFX 재생
    // 해당 스크립트 update 에서 SoundManager.instance.PlayPlayerAttackSFX(); 이런식으로 넣어주면 됨

    public void PlayPlayerAttackSFX()
    {
        PlaySFX(cannon);
    }

    public void PlayEnemyAttackSFX()
    {
        PlaySFX(hit);
    }

    public void PlaySellSFX()
    {
        PlaySFX(buySell);
    }

    public void PlayDontSellSFX()
    {
        PlaySFX(denied);
    }
}










