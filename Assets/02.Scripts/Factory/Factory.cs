using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    public static Factory Instance { get; private set; } // �̱��� �ν��Ͻ�

    public FactoryData data;
    public Text mainSceneCoinsTxt; // ���ξ� ��ܹ� ���� ��������
    CharacterStats characterStats;
    Inventory inventory;

    public int currentLevel { get => data.currentLevel; set => data.currentLevel = value; }     // ���� ����
    public int nextLevel { get => data.nextLevel; set => data.nextLevel = value; }      // ���� ����
    public int currentCoins { get => characterStats.coin; set { characterStats.coin = value; data.currentCoins = value; } }     // ���� �������ִ� ����
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
            //GameObject.DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        characterStats = FindObjectOfType<CharacterStats>();
        inventory = FindObjectOfType<Inventory>();
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
        // currentLevel = coinsPerSec + 5000; // 1���� ���������� ���귮 ����... �� �ɸ���
        coinsPerSec = currentLevel * 5000; // 1���� ���������� ���귮 ����
    }

    public void LoadData()     // �ҷ��� ������
    {
        FactoryData data = DataManager.Instance.LoadData<FactoryData>();

        if (data == null)   // �����Ͱ� null�̶��
        {
            // �ʱⰪ ����
            currentLevel = 1;
            nextLevel = 2;
            currentCoins = characterStats.coin;
            upgradeCost = 50000;
        }
        else    // null�� �ƴ϶�� ����� ������ �ҷ��ͼ� ���� ����
        {
            currentLevel = data.currentLevel;
            nextLevel = data.nextLevel;
            currentCoins = data.currentCoins;
            upgradeCost = data.upgradeCost;
        }

        // !!! �ʱⰪ �����ϴ°� !!! �ڡڡڡڡڡڡ�
        //currentLevel = 1;
        //nextLevel = 2;
        //currentCoins = 50000;
        //upgradeCost = 50000;
        // coinsPerSec = 5000; 
    }

    public void SaveData()     // ������ ������
    {
        FactoryData data = new FactoryData();
        
        data.currentLevel = currentLevel;
        data.nextLevel = nextLevel;
        data.currentCoins = characterStats.coin;
        data.upgradeCost = upgradeCost;

        DataManager.Instance.SaveData(data);
    }

    private void OnApplicationQuit()    // ������ ���� �� ����
    {
        SaveData();
        inventory.SaveData();   // �κ��丮���� ������ �ȵż� �ӽ� �ڵ�
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
