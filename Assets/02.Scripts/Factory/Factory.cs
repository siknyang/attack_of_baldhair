using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    public static Factory Instance { get; private set; } // �̱��� �ν��Ͻ�

    public FactoryData data;
    public Text mainSceneCoinsTxt; // ���ξ� ��ܹ� ���� ��������

    public int currentLevel { get => data.currentLevel; set => data.currentLevel = value; }     // ���� ����
    public int nextLevel { get => data.nextLevel; set => data.nextLevel = value; }      // ���� ����
    public int currentCoins { get => data.currentCoins; set => data.currentCoins = value; }     // ���� �������ִ� ����
    public int coinsPerSec;    // 1�ʿ� �����Ǵ� ����
    public int upgradeCost { get => data.upgradeCost; set => data.upgradeCost = value; }    // ���� ������ �������� �ʿ��� ����

    public Text currentLevelText;
    public Text nextLevelText;
    public Text coinsText; // ( currentCoins / upgradeCost )
    public Button upgradeButton;

    private bool canUpgrade; // ���׷��̵� ��ư

    private void Awake()
    {
        // �̱��� �ν��Ͻ� ����
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
        LoadData();     // ������ ������ �� ����� ������ �ҷ�����
        UpdateCoinsPerSec(); // coinsPerSec �� ������Ʈ
        InvokeRepeating("ProduceCoins", 1.0f, 1.0f); // (�ʱ� �����ð�, �ݺ�����)
        UpdateUI();
    }

    void ProduceCoins()
    {
        currentCoins += coinsPerSec;
        UpdateUI();
    }

    void UpdateUI()
    {
        currentLevelText.text = "���� ���� " + currentLevel;
        nextLevelText.text = "���� ���� " + nextLevel;

        coinsText.text = $"<color=#FF0000>{currentCoins}</color> / {upgradeCost}";
        //coinsText.text = string.Format("{0} / {1}", currentCoins, upgradeCost);
        // string.Format���� ���ٿ� �Է��ϱ�

        if (mainSceneCoinsTxt != null)
        {
            mainSceneCoinsTxt.text = currentCoins + "C";
        }

        //canUpgrade = currentCoins >= upgradeCost;   // ���纸������ >= �ʿ����� �̸� ���׷��̵� ����
        //upgradeButton.interactable = canUpgrade;    // ���׷��̵� ��ư Ȱ��ȭ
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
            upgradeCost += 50000; // ���׷��̵� ��� ����
            UpdateCoinsPerSec(); // ������ �ö󰡸� coinPerSec ���� ������Ʈ
            SaveData();
            UpdateUI();
        }
    }

    void UpdateCoinsPerSec()
    {
        coinsPerSec = coinsPerSec * 2; // 1���� ���������� ���귮 �ι� ����
    }

    public void LoadData()     // �ҷ��� ������
    {
        FactoryData data = DataManager.Instance.LoadData<FactoryData>();

        if (data == null)   // �����Ͱ� null�̶��
        {
            // �ʱⰪ ����
            currentLevel = 1;
            nextLevel = 2;
            currentCoins = 0;
            upgradeCost = 50000;
        }
        else    // null�� �ƴ϶�� ����� ������ �ҷ��ͼ� ���� ����
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

    public void SaveData()     // ������ ������
    {
        FactoryData data = new FactoryData();
        
        data.currentLevel = 1;
        data.nextLevel = nextLevel;
        data.currentCoins = 0;
        data.upgradeCost = upgradeCost;

        DataManager.Instance.SaveData(data);
    }

    private void OnApplicationQuit()    // ������ ���� �� ����
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
