using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioMixer mixer; // BGM�� SFX ������ ������ Audio Mixer

    // BGM ���� Ŭ����
    public AudioClip casualSuspense;
    public AudioClip casualTheme3Loop;
    public AudioClip tropicalGame;

    // SFX ���� Ŭ����
    public AudioClip hover;
    public AudioClip denied;
    public AudioClip unequip;
    public AudioClip buySell;
    public AudioClip hit;
    public AudioClip teleport;
    public AudioClip cannon;
    public AudioClip magic;

    private AudioSource bgmSource; // BGM�� ����� AudioSource
    private AudioSource sfxSource; // BGM�� ����� AudioSource


    public Slider bgmSlider; // BGM ������ ������ �����̴�
    public Slider sfxSlider; // SFX ������ ������ �����̴�

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

        // AudioSource ������Ʈ ����
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true; // BGM�� �ݺ� ����ϵ��� ����

        sfxSource = gameObject.AddComponent<AudioSource>(); // SFX�� ����� AudioSource �߰�
        sfxSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFXVolume")[0]; // SFX�� Audio Mixer �׷� ����
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Scene�� �ε�� �� ȣ���� �Լ� ����

        InitializeSliders();
        ApplySavedVolumes();
    }

    void InitializeSliders()
    {
        // �����̴� �ʱ�ȭ
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
        // ����� ���� ����
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

    // Scene�� �ε�� �� ȣ��Ǵ� �Լ�
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Scene �̸��� ���� BGM ���
        switch (scene.name)
        {
            case "IntroScene":
                PlayBGM(casualSuspense);
                break;
            case "Yuyerin":
                PlayBGM(casualTheme3Loop);
                break;
                // �߰����� Scene�� ������ ���⿡ �߰��� �� �ֽ��ϴ�.
        }

        // �����̴� ���� �缳��
        InitializeSliders();
        ApplySavedVolumes();
    }

    // BGM ���
    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmClip == null) return;

        bgmSource.clip = bgmClip;
        bgmSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BGMVolume")[0]; // Audio Mixer ���� ����
        bgmSource.Play();
    }

    // SFX ���
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip == null) return;

        AudioSource.PlayClipAtPoint(sfxClip, Camera.main.transform.position);
    }

    // ���� ����
    public void SetBGMVolume(float volume)
    {
        if (volume == 0)
        {
            mixer.SetFloat("BGMVolume", -80f); // ������ �ּҷ� ���� (���Ұ�)
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
            mixer.SetFloat("SFXVolume", -80f); // ������ �ּҷ� ���� (���Ұ�)
        }
        else
        {
            mixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    // ��ư Ŭ�� SFX ���
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

    // Ư�� ���� SFX ���
    // �ش� ��ũ��Ʈ update ���� SoundManager.instance.PlayPlayerAttackSFX(); �̷������� �־��ָ� ��

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










