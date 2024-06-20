using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Factory : MonoBehaviour
{
    public int currentLevel = 1;    // ���� ����
    public int nextLevel = 2;       // ���� ����
    public int currentCoins = 0;    // ���� �������ִ� ����
    public int coinsPerSec = 100;   // 1�ʿ� �����Ǵ� ���� (1�ʴ� 100����)
    public int upgradeCost = 50000; // ���� ������ �������� �ʿ��� ����

    public Text currentLevelText;
    public Text nextLevelText;
    public Text coinsText;
    public Button upgradeButton;

    private bool canUpgrade;

    void Start()
    {
        InvokeRepeating("ProduceCoins", 1.0f, 1.0f); // (�ʱ� �����ð�, �ݺ�����)
        // InvokeRepeating("ProduceCoins", 1, 1); // (.0f�� �Ƚᵵ ���ư����ϴ��� ��������)

        UpdateUI();
    }

    void ProduceCoins()
    {
        currentCoins += coinsPerSec;
        UpdateUI();
    }

    void UpdateUI()
    {
        currentLevelText.text = "���� " + currentLevel;
        nextLevelText.text = "���� ���� " + nextLevel;
        coinsText.text = string.Format("{0} / {1}", currentCoins, upgradeCost);
        // string.Format���� ���ٿ� �Է��ϱ�

        canUpgrade = currentCoins >= upgradeCost;   // ���纸������ >= �ʿ����� �̸� ���׷��̵� ����
        upgradeButton.interactable = canUpgrade;    // ���׷��̵� ��ư Ȱ��ȭ
    }

    public void Upgrade()
    {
        if (canUpgrade)
        {
            Debug.Log("���׷��̵��ư");
            currentCoins -= upgradeCost;
            currentLevel++;
            nextLevel++;
            upgradeCost += 50000; // ���׷��̵� ��� ����
            UpdateUI();
        }
    }
}
