using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItem[] shopItems;    // ���� ������ �迭
    public Inventory inventory;     // �κ��丮 ����

    public ItemSO[] items;

    //public string[] itemNames;      // ������ �̸� �迭
    //public string[] itemInfo;       // ������ ���� �迭
    //public int[] itemPrices;        // ������ ���� �迭
    //public Sprite[] itemImages;     // ������ �̹��� �迭

    void Start()
    {
        InitializeShop();   // ���� �ʱ�ȭ
    }

    void InitializeShop()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            if (i < items.Length)
            {
                shopItems[i].Initialize(items[i], inventory);
            }
            else
            {
                Debug.Log("������ �迭 ��ġX");
            }
        }
    }
}
