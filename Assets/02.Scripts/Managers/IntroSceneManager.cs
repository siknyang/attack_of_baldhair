using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    public InputField nicknameInputField;

    void Start()
    {
        SoundManager.instance.PlayBGM(SoundManager.instance.casualSuspense);
    }
    public void OnStartButtonClicked()
    {
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            nickname = "Bold Bald"; // �г��� �Է� ���ϰ� �Ѿ�� �⺻�� �̸�
        }

        PlayerPrefs.SetString("PlayerNickname", nickname);
        PlayerPrefs.Save(); // ������ ��������� ȣ��

       

        SceneManager.LoadScene("MainScene"); // ��ư ������ �� �Ѿ��


    }
}

