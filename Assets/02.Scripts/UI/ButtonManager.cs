using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Dungeon1Btn()
    {
        DungeonManager.Instance.buttonClicked = "Dungeon1Btn";
        SceneManager.LoadScene("DungeonScene");
    }

    public void Dungeon2Btn()
    {
        DungeonManager.Instance.buttonClicked = "Dungeon2Btn";
        SceneManager.LoadScene("DungeonScene");
    }

    public void Dungeon3Btn()
    {
        DungeonManager.Instance.buttonClicked = "Dungeon3Btn";
        SceneManager.LoadScene("DungeonScene");
    }

    public void TestBtn()
    {
        SceneManager.LoadScene("Yuyerin");
    }

}
