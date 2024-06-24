using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    // 슬롯 관리하는 곳. 각 슬롯에 아이템 넣기

    public InventorySlot[] slots;   // 여러개의 인벤토리 슬롯을 저장하는 배열
    public Sprite[] itemImages;     // 아이템 이미지 배열
    public string[] itemNames;      // 아이템 이름 배열
    public string[] itemInfo;       // 아이템 설명 배열
    public Text emptyText;          // 인벤토리가 비어있을 때 표시하는 텍스트
    public Player player;           // Player 추가

    private Dictionary<string, int> itemCounts = new Dictionary<string, int>(); // 아이템 이름을 키로 하고, 수량을 값으로 가지는 딕셔너리

    void Start()
    {  
        slots = GetComponentsInChildren<InventorySlot>();
        InitializeSlots();
        UpdateEmptyText();
    }

    void InitializeSlots() // 모든 슬롯을 "가방이 비어있습니다"로 초기화
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
            slots[i].characterStats = player; // 슬롯에 player의 CharacterStats 연결
        }
    }

    // 아이템을 인벤토리에 추가
    public void AddItem(string name, string info, Sprite image)
    {
        if (itemCounts.ContainsKey(name))
        {
            itemCounts[name]++;
        }
        else
        {
            itemCounts[name] = 1;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemName.text == "가방이 비어있습니다")
                {
                    slots[i].InitializeSlot(image, name, info);
                    Debug.Log($"Item added : {name} {info}");
                    break;
                }
            }
        }
        UpdateItemCount(name);
        UpdateEmptyText();
    }

    private void UpdateItemCount(string name)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName.text == name)
            {
                slots[i].UpdateItemCount(itemCounts[name]);
                break;
            }
        }
    }

    private void UpdateEmptyText()
    {
        bool isEmpty = true;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName.text != "가방이 비어있습니다")
            {
                isEmpty = false;
                break;
            }
        }
        emptyText.gameObject.SetActive(isEmpty);
    }

}
