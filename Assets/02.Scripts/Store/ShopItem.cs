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

    //private string itemNameValue;
    //private string itemInfoValue;
    //private int itemPriceValue;
    //private Sprite itemImageValue;

    private ItemSO itemData;
    private Inventory inventory;    // 인벤토리 참조

    // 아이템 초기화
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
        UpdateBuyButtonState(); // 초기상태 업데이트
    }

    private void BuyItem()
    {
        if (Factory.Instance.currentCoins >= itemData.itemPrice)
        {
            Factory.Instance.currentCoins -= itemData.itemPrice; // 코인 차감
            inventory.AddItem(itemData);
            // 상점에서 구매버튼을 누르면 인벤토리에 아이템을 추가
            // 아이템을 인벤토리에 추가할때마다 해당 아이템 수 증가
            UpdateBuyButtonState(); // 구매 후 버튼 상태 업데이트
        }

    }

    void UpdateBuyButtonState()
    {
        if (Factory.Instance != null)
        {
            //if (Factory.Instance.currentCoins >= itemPriceValue)
            //{
            //    buyButton.interactable = true; // 코인 충분하면 버튼 활성화
            //}
            //else
            //{
            //    buyButton.interactable = false; // 코인 부족하면 버튼 비활성화
            //}
            buyButton.interactable = Factory.Instance.currentCoins >= itemData.itemPrice;
        }
        else
        {
            buyButton.interactable = false; // Factory.Instance가 null이면 버튼 비활성화
        }
    }

    void Update()
    {
        UpdateBuyButtonState(); // 버튼 상태 업데이트    
    }

}
