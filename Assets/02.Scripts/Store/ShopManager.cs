using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItem[] shopItems;
    public Inventory inventory;

    public string[] itemNames;
    public string[] itemInfo;
    public int[] itemPrices;
    public Sprite[] itemImages;

    void Start()
    {
        InitializeShop();   
    }

    void InitializeShop()
    {
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].Initialize(itemNames[i], itemInfo[i], itemPrices[i], itemImages[i], inventory);
        }
    }
}
