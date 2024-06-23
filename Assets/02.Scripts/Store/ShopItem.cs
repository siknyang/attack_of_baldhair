using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // 상점 아이템 관리. 구매버튼 누르면 아이템을 인벤토리로 추가.

    public Image itemImage;
    public Text itemName;
    public Text itemInfo;
    public Text itemPrice;
    public Button buyButton;

    private string itemNameValue;
    private string itemInfoValue;
    private int itemPriceValue;
    private Sprite itemImageValue;
    private Inventory inventory;

    // 아이템 초기화
    public void Initialize(string name, string info, int price, Sprite image, Inventory inventory)
    {
        itemNameValue = name;
        itemInfoValue = info;
        itemPriceValue = price;
        itemImageValue = image;
        this.inventory = inventory;

        itemName.text = itemNameValue;
        itemInfo.text = itemInfoValue;
        itemPrice.text = itemPriceValue.ToString() + "C";
        itemImage.sprite = itemImageValue;

        buyButton.onClick.AddListener(BuyItem);
    }

    private void BuyItem()
    {
        inventory.AddItem(itemNameValue, itemInfoValue, itemImageValue);
        // 상점에서 구매버튼을 누르면 인벤토리에 아이템을 추가
    }
}
