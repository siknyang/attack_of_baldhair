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

    public Button equipButton;
    private bool isEquipped = false; // 현재 아이템 장착 상태
    // false = 해제된 상태, true = 장착된 상태

    void Start()
    {
        equipButton.onClick.AddListener(ToggleEquip);
        // 장착버튼에 클릭 이벤트로 추가

        // UpdateButton(); // 여기서도 업데이트를 해줘야되나...
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
            UnequipItem(); // 아이템 해제 호출
        }
    }

    void UpdateButton()
    {
        // 장착 상태에 따라 해제 / 장착 으로 텍스트 변경됨
        equipButton.GetComponentInChildren<Text>().text = isEquipped ? "해제" : "장착";
    }

    void EquipItem()
    {
        // 아이템 장착 로직 추가
    }

    void UnequipItem()
    {
        // 아이템 해제 로직 추가
    }
}
