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


    // 아이템 장착 시 스탯 증가
    public void IncreaseStats(string itemName)
    {
        switch (itemName)
        {
            case "아이템1":
                attackPower += 10;
                break;

            case "아이템2":
                attackPower += 10;
                break;

            case "아이템3":
                attackPower += 10;
                break;

            default:
                break;
        }
    }

    // 아이템 해제 시 스탯 감소
    public void DecreaseStats(string itemName)
    {
        switch (itemName)
        {
            case "아이템1":
                attackPower -= 10;
                break;

            case "아이템2":
                attackPower -= 10;
                break;

            case "아이템3":
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