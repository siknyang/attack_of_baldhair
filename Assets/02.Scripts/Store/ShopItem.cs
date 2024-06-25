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

    //private string itemNameValue;
    //private string itemInfoValue;
    //private int itemPriceValue;
    //private Sprite itemImageValue;

    private ItemSO itemData;
    private Inventory inventory;    // �κ��丮 ����

    // ������ �ʱ�ȭ
    public void Initialize(ItemSO item, Inventory inventory)
    {
        //itemNameValue = name;
        //itemInfoValue = info;
        //itemPriceValue = price;
        //itemImageValue = image;
        this.itemData = item;
        this.inventory = inventory;

        itemName.text = item.itemName;
        itemInfo.text = item.itemDescription;
        itemPrice.text = item.itemPrice.ToString() + "C";
        itemImage.sprite = item.icon;

        buyButton.onClick.AddListener(BuyItem);
        UpdateBuyButtonState(); // �ʱ���� ������Ʈ
    }

    private void BuyItem()
    {
        if (Factory.Instance.currentCoins >= itemData.itemPrice)
        {
            Factory.Instance.currentCoins -= itemData.itemPrice; // ���� ����
            inventory.AddItem(itemData);
            // �������� ���Ź�ư�� ������ �κ��丮�� �������� �߰�
            // �������� �κ��丮�� �߰��Ҷ����� �ش� ������ �� ����
            UpdateBuyButtonState(); // ���� �� ��ư ���� ������Ʈ
        }

    }

    void UpdateBuyButtonState()
    {
        if (Factory.Instance != null)
        {
            //if (Factory.Instance.currentCoins >= itemPriceValue)
            //{
            //    buyButton.interactable = true; // ���� ����ϸ� ��ư Ȱ��ȭ
            //}
            //else
            //{
            //    buyButton.interactable = false; // ���� �����ϸ� ��ư ��Ȱ��ȭ
            //}
            buyButton.interactable = Factory.Instance.currentCoins >= itemData.itemPrice;
        }
        else
        {
            buyButton.interactable = false; // Factory.Instance�� null�̸� ��ư ��Ȱ��ȭ
        }
    }

    void Update()
    {
        UpdateBuyButtonState(); // ��ư ���� ������Ʈ    
    }

}
