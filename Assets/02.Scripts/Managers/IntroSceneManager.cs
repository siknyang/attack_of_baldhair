using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSceneManager : MonoBehaviour
{
    public InputField nicknameInputField;

    public void OnStartButtonClicked()
    {
        string nickname = nicknameInputField.text;

        if (string.IsNullOrEmpty(nickname))
        {
            nickname = "Bold Bald"; // 닉네임 입력 안하고 넘어가면 기본값 이름
        }

        PlayerPrefs.SetString("PlayerNickname", nickname);
        PlayerPrefs.Save(); // 저장을 명시적으로 호출
        SceneManager.LoadScene("Yuyerin"); // 버튼 누르면 씬 넘어가게
    }
}

