using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLV_EXP : MonoBehaviour
{
    [SerializeField] private Text levelText;
    //[SerializeField] private Image experienceSlider;
    //[SerializeField] private Text experienceText;

    [SerializeField] private Text attackPowerText;
    [SerializeField] private Text attackSpeedText;


    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        if (player != null)
        {
            player.OnExperienceChanged += UpdateExperienceUI;
            player.OnLevelChanged += UpdateLevelUI;
            //player.OnCoinChanged += UpdateCoinUI;
            //UpdateExperienceUI();
            UpdateLevelUI();
            
        }
    }

    private void UpdateExperienceUI()
    {
        if (player == null) return;

        //experienceSlider.maxValue = player.experienceToNextLevel;
        //experienceSlider.value = player.experience;
        //experienceText.text = $"{player.experience}/{player.experienceToNextLevel}";
    }

    private void UpdateLevelUI()
    {
        if (player == null) return;

        levelText.text = player.level.ToString();
        attackPowerText.text = player.attackPower.ToString();
        attackSpeedText.text = player.attackSpeed.ToString();
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnExperienceChanged -= UpdateExperienceUI;
            player.OnLevelChanged -= UpdateLevelUI;
        }
    }
}
