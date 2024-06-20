using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    public int currentLevel = 1;    // 현재 레벨
    public int nextLevel = 2;       // 다음 레벨
    public int currentCoins = 0;    // 현재 가지고있는 코인
    public int coinsPerSec = 100;   // 1초에 생성되는 코인 (1초당 100코인)
    public int upgradeCost = 50000; // 다음 레벨로 가기위해 필요한 코인

    public Text currentLevelText;
    public Text nextLevelText;
    public Text coinsText;
    public Button upgradeButton;

    private bool canUpgrade;

    void Start()
    {
        InvokeRepeating("ProduceCoins", 1.0f, 1.0f); // (초기 지연시간, 반복간격)
        // InvokeRepeating("ProduceCoins", 1, 1); // (.0f를 안써도 돌아가긴하던데 무슨차이)

        UpdateUI();
    }

    void ProduceCoins()
    {
        currentCoins += coinsPerSec;
        UpdateUI();
    }

    void UpdateUI()
    {
        currentLevelText.text = "레벨 " + currentLevel;
        nextLevelText.text = "다음 레벨 " + nextLevel;
        coinsText.text = string.Format("{0} / {1}", currentCoins, upgradeCost);
        // string.Format으로 한줄에 입력하기

        canUpgrade = currentCoins >= upgradeCost;   // 현재보유코인 >= 필요코인 이면 업그레이드 가능
        upgradeButton.interactable = canUpgrade;    // 업그레이드 버튼 활성화
    }

    public void Upgrade()
    {
        if (canUpgrade)
        {
            Debug.Log("업그레이드버튼");
            currentCoins -= upgradeCost;
            currentLevel++;
            nextLevel++;
            upgradeCost += 50000; // 업그레이드 비용 증가
            UpdateUI();
        }
    }
}
