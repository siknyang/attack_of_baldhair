using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // ���� ������ ����. ���Ź�ư ������ �������� �κ��丮�� �߰�.

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

    // ������ �ʱ�ȭ
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
        // �������� ���Ź�ư�� ������ �κ��丮�� �������� �߰�
    }
}
