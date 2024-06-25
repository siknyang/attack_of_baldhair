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
    private Inventory inventory;    // �κ��丮 ����

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
        UpdateBuyButtonState(); // �ʱ���� ������Ʈ
    }

    private void BuyItem()
    {
        if (Factory.Instance.currentCoins >= itemPriceValue)
        {
            Factory.Instance.currentCoins -= itemPriceValue; // ���� ����
            inventory.AddItem(itemNameValue, itemInfoValue, itemImageValue);
            // �������� ���Ź�ư�� ������ �κ��丮�� �������� �߰�
            // �������� �κ��丮�� �߰��Ҷ����� �ش� ������ �� ����
            UpdateBuyButtonState(); // ���� �� ��ư ���� ������Ʈ
        }

    }

    void UpdateBuyButtonState()
    {
        if (Factory.Instance != null)
        {
            if (Factory.Instance.currentCoins >= itemPriceValue)
            {
                buyButton.interactable = true; // ���� ����ϸ� ��ư Ȱ��ȭ
            }
            else
            {
                buyButton.interactable = false; // ���� �����ϸ� ��ư ��Ȱ��ȭ
            }
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
