using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // 슬롯 1칸 안에있는 요소(이미지,이름,정보)를 관리하는 클래스 + 장착/해제 기능

    public Image itemImage;
    public Text itemName;
    public Text itemInfo;
    public Text itemCount; // 아이템 수량표시 추가★
    public Button equipButton;
    private bool isEquipped = false; // 현재 아이템 장착 상태
    // false = 해제된 상태, true = 장착된 상태

    public CharacterStats characterStats; // 캐릭터 스탯 클래스

    public ItemSO currentItem;

    void Start()
    {
        if (equipButton != null)
        {
            equipButton.onClick.AddListener(ToggleEquip);
            // 장착버튼에 클릭 이벤트로 추가
        }
    }

    public void InitializeSlot(ItemSO item)
    {
        currentItem = item;
        itemImage.sprite = item.icon;
        itemName.text = item.itemName;
        itemInfo.text = item.itemDescription;
        itemCount.text = "0";
        gameObject.SetActive(true); // 슬롯 활성화
        // Debug.Log($"Slot initialized: {name}, {info}");
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemName.text = "가방이 비어있습니다";
        itemInfo.text = "아이템을 구매하세요";
        itemCount.text = "0";
        gameObject.SetActive(true);
    }

    public void UpdateItemCount(int count)
    {
        itemCount.text = count.ToString();
    }

    void ToggleEquip()
    {
        isEquipped = !isEquipped; // 장착상태 반전시키기
        UpdateButton();

        if (isEquipped ) // false 일때
        {
            EquipItem(); // 아이템 장착 호출
        }
        else // true 일때
        {
            UnEquipItem(); // 아이템 해제 호출
        }
    }

    void UpdateButton()
    {
        // 장착 상태에 따라 해제 / 장착 으로 텍스트 변경됨
        equipButton.GetComponentInChildren<Text>().text = isEquipped ? "해제" : "장착";
    }

    void EquipItem()
    {
        if (characterStats != null && currentItem != null)
        {
            switch (currentItem.itemName)
            {
                case "트리트먼트":
                    characterStats.IncreaseAttackSpeed(5.0f);
                    break;
                case "손":
                    characterStats.IncreaseAttackPower(10.0f);
                    break;
                default:
                    break;
            }
            Debug.Log($"아이템 장착 : {currentItem.itemName}");
        }
    }

    void UnEquipItem()
    {
        if (characterStats != null && currentItem != null)
        {
            switch (currentItem.itemName)
            {
                case "트리트먼트":
                    characterStats.IncreaseAttackSpeed(-5.0f);
                    break;
                case "손":
                    characterStats.IncreaseAttackPower(-10.0f);
                    break;
                default:
                    break;
            }
            Debug.Log($"아이템 해제 : {currentItem.itemName}");
        }
    }

    //void EquipItem()
    //{
    //    // 아이템 장착 로직 추가
    //    if (characterStats != null && currentItem != null)
    //    {
    //        characterStats.IncreaseStats(currentItem.itemName);
    //        Debug.Log($"아이템 장착 : {currentItem.itemName}");
    //    }
    //}

    //void UnequipItem()
    //{
    //    // 아이템 해제 로직 추가
    //    if (characterStats != null && currentItem != null)
    //    {
    //        characterStats.DecreaseStats(currentItem.itemName);
    //        Debug.Log($"아이템 해제 : {currentItem.itemName}");
    //    }
    //}
}
