using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // 슬롯 관리하는 곳. 각 슬롯에 아이템 넣기

    public InventorySlot[] slots;   // 여러개의 인벤토리 슬롯을 저장하는 배열
    public Sprite[] itemImages;     // 아이템 이미지 배열
    public string[] itemNames;      // 아이템 이름 배열
    public string[] itemInfo;       // 아이템 설명 배열

    void Start()
    {  
        InitializeSlots();
    }

    void InitializeSlots() // 슬롯 초기화 메서드
    {
        // 배열을 순회하면서 각 슬롯에 아이템 이미지 설정
        for (int i = 0; i < slots.Length; i++) // i = 인덱스 변수. 초기값 0; slot 배열의 길이보다 작을때까지만 반복; 매 반복마다 1씩 증가.
        {
            if (i < itemImages.Length) // 배열의 길이보다 작다면
            {
                // slots[i] 는 slots 배열의 i번째를 의미
                slots[i].itemImage.sprite = itemImages[i]; // itemImage.sprite에 이미지 설정
                slots[i].itemName.text = itemNames[i]; // itemName.text에 이름 설정
                slots[i].itemInfo.text = itemInfo[i]; // itemInfo.text에 정보 설정
                slots[i].gameObject.SetActive(true); // 슬롯의 gameObject 활성화
            }
            else // i가 itemImages 배열의 길이보다 크거나 같을때
            {
                slots[i].gameObject.SetActive(false); // 정보가 없다면 남은 슬롯 비활성화
            }
        }
    }

    // 아이템을 인벤토리에 추가하는 메서드
    public void AddItem(string name, string info, Sprite image)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].gameObject.activeInHierarchy)
            {
                slots[i].itemImage.sprite = image;
                slots[i].itemName.text = name;
                slots[i].itemInfo.text = info;
                slots[i].gameObject.SetActive(true);
                break;
            }
        }
    }
}
