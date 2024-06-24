using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    // ���� �����ϴ� ��. �� ���Կ� ������ �ֱ�

    public InventorySlot[] slots;   // �������� �κ��丮 ������ �����ϴ� �迭
    public Sprite[] itemImages;     // ������ �̹��� �迭
    public string[] itemNames;      // ������ �̸� �迭
    public string[] itemInfo;       // ������ ���� �迭
    public Text emptyText;          // �κ��丮�� ������� �� ǥ���ϴ� �ؽ�Ʈ
    public Player player;           // Player �߰�

    private Dictionary<string, int> itemCounts = new Dictionary<string, int>(); // ������ �̸��� Ű�� �ϰ�, ������ ������ ������ ��ųʸ�

    void Start()
    {  
        slots = GetComponentsInChildren<InventorySlot>();
        InitializeSlots();
        UpdateEmptyText();
    }

    void InitializeSlots() // ��� ������ "������ ����ֽ��ϴ�"�� �ʱ�ȭ
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
            slots[i].characterStats = player; // ���Կ� player�� CharacterStats ����
        }
    }

    // �������� �κ��丮�� �߰�
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
                if (slots[i].itemName.text == "������ ����ֽ��ϴ�")
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
            if (slots[i].itemName.text != "������ ����ֽ��ϴ�")
            {
                isEmpty = false;
                break;
            }
        }
        emptyText.gameObject.SetActive(isEmpty);
    }

}
