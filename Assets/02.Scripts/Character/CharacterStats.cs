using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public UserData data;

    public string nickName { get => data.nickName; set => data.nickName = value; }
    public int level { get => data.level; set => data.level = value; }
    public float health;
    public float attackPower { get => data.attackPower; set => data.attackPower = value; }
    public float attackSpeed { get => data.attackSpeed; set => data.attackSpeed = value; }
    public float moveSpeed;
    public float attackRange;
    public float experience { get => data.experience; set => data.experience = value; }
    public int coin { get => data.coin; set => data.coin = value; }


    // ������ ���� �� ���� ����
    public void IncreaseStats(string itemName)
    {
        switch (itemName)
        {
            case "������1":
                attackPower += 10;
                break;

            case "������2":
                attackPower += 10;
                break;

            case "������3":
                attackPower += 10;
                break;

            default:
                break;
        }
    }

    // ������ ���� �� ���� ����
    public void DecreaseStats(string itemName)
    {
        switch (itemName)
        {
            case "������1":
                attackPower -= 10;
                break;

            case "������2":
                attackPower -= 10;
                break;

            case "������3":
                attackPower -= 10;
                break;

            default:
                break;
        }
    }

}

[System.Serializable]
public class UserData
{
    public string nickName;
    public int level;
    public float attackPower;
    public float attackSpeed;
    public float experience;
    public int coin;
}