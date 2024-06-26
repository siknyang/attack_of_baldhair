using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItem[] shopItems;    // 상점 아이템 배열
    public Inventory inventory;     // 인벤토리 참조

    public ItemSO[] items;

    //public string[] itemNames;      // 아이템 이름 배열
    //public string[] itemInfo;       // 아이템 정보 배열
    //public int[] itemPrices;        // 아이템 가격 배열
    //public Sprite[] itemImages;     // 아이템 이미지 배열

    void Start()
    {
        InitializeShop();   // 상점 초기화
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
                Debug.Log("아이템 배열 일치X");
            }
        }
    }
}
