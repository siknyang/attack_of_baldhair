using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneManager : MonoBehaviour
{
    public Text nicknameText;

    void Start()
    {
        string nickname = PlayerPrefs.GetString("PlayerNickname", "Nickname");
        nicknameText.text = nickname;
    }
}
