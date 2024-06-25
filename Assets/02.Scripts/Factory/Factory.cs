using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    public static Factory Instance { get; private set; } // 싱글톤 인스턴스

    public FactoryData data;
    public Text mainSceneCoinsTxt; // 메인씬 상단바 현재 보유코인

    public int currentLevel { get => data.currentLevel; set => data.currentLevel = value; }     // 현재 레벨
    public int nextLevel { get => data.nextLevel; set => data.nextLevel = value; }      // 다음 레벨
    public int currentCoins { get => data.currentCoins; set => data.currentCoins = value; }     // 현재 가지고있는 코인
    public int coinsPerSec;    // 1초에 생성되는 코인
    public int upgradeCost { get => data.upgradeCost; set => data.upgradeCost = value; }    // 다음 레벨로 가기위해 필요한 코인

    public Text currentLevelText;
    public Text nextLevelText;
    public Text coinsText; // ( currentCoins / upgradeCost )
    public Button upgradeButton;

    private bool canUpgrade; // 업그레이드 버튼

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadData();     // 게임이 시작할 때 저장된 데이터 불러오기
        UpdateCoinsPerSec(); // coinsPerSec 값 업데이트
        InvokeRepeating("ProduceCoins", 1.0f, 1.0f); // (초기 지연시간, 반복간격)
        UpdateUI();
    }

    void ProduceCoins()
    {
        currentCoins += coinsPerSec;
        UpdateUI();
    }

    void UpdateUI()
    {
        currentLevelText.text = "공장 레벨 " + currentLevel;
        nextLevelText.text = "다음 레벨 " + nextLevel;

        coinsText.text = $"<color=#FF0000>{currentCoins}</color> / {upgradeCost}";
        //coinsText.text = string.Format("{0} / {1}", currentCoins, upgradeCost);
        // string.Format으로 한줄에 입력하기

        if (mainSceneCoinsTxt != null)
        {
            mainSceneCoinsTxt.text = currentCoins + "C";
        }

        //canUpgrade = currentCoins >= upgradeCost;   // 현재보유코인 >= 필요코인 이면 업그레이드 가능
        //upgradeButton.interactable = canUpgrade;    // 업그레이드 버튼 활성화
        if (currentCoins >= upgradeCost)
        {
            canUpgrade = true;
            upgradeButton.interactable = true;
        }
        else
        {
            canUpgrade = false;
            upgradeButton.interactable = false;
        }
    }

    public void Upgrade()
    {
        if (canUpgrade)
        {
            currentCoins -= upgradeCost;
            currentLevel++;
            nextLevel++;
            upgradeCost += 50000; // 업그레이드 비용 증가
            UpdateCoinsPerSec(); // 레벨이 올라가면 coinPerSec 값을 업데이트
            SaveData();
            UpdateUI();
        }
    }

    void UpdateCoinsPerSec()
    {
        coinsPerSec = coinsPerSec * 2; // 1레벨 오를때마다 생산량 두배 증가
    }

    public void LoadData()     // 불러온 데이터
    {
        FactoryData data = DataManager.Instance.LoadData<FactoryData>();

        if (data == null)   // 데이터가 null이라면
        {
            // 초기값 세팅
            currentLevel = 1;
            nextLevel = 2;
            currentCoins = 0;
            upgradeCost = 50000;
        }
        else    // null이 아니라면 저장된 데이터 불러와서 덮어 씌움
        {
            currentLevel = data.currentLevel;
            nextLevel = data.nextLevel;
            currentCoins = data.currentCoins;
            upgradeCost = data.upgradeCost;
        }
        currentLevel = 1;
        nextLevel = 2;
        currentCoins = 0;
        upgradeCost = 50000;
    }

    public void SaveData()     // 저장할 데이터
    {
        FactoryData data = new FactoryData();
        
        data.currentLevel = 1;
        data.nextLevel = nextLevel;
        data.currentCoins = 0;
        data.upgradeCost = upgradeCost;

        DataManager.Instance.SaveData(data);
    }

    private void OnApplicationQuit()    // 게임이 끝날 때 저장
    {
        SaveData();
    }
}

[System.Serializable]
public class FactoryData
{
    public int currentLevel;
    public int nextLevel;
    public int currentCoins;
    public int upgradeCost;
}
