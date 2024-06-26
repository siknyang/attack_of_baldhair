using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour
{
    // ���� �����ϴ� ��. �� ���Կ� ������ �ֱ�

    public InventorySlot[] slots;   // �������� �κ��丮 ������ �����ϴ� �迭

    /*
    public Sprite[] itemImages;     // ������ �̹��� �迭
    public string[] itemNames;      // ������ �̸� �迭
    public string[] itemInfo;       // ������ ���� �迭 => ItemSO
    */

    public Text emptyText;          // �κ��丮�� ������� �� ǥ���ϴ� �ؽ�Ʈ
    public Player player;           // Player �߰�

    private Dictionary<string, int> itemCounts = new Dictionary<string, int>(); // ������ �̸��� Ű�� �ϰ�, ������ ������ ������ ��ųʸ�

    public InvenData data;

    void Start()
    {
        LoadData();
        slots = GetComponentsInChildren<InventorySlot>();
        InitializeSlots();
        UpdateEmptyText();
        gameObject.SetActive(false);
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
    public void AddItem(ItemSO item)
    {
        if (itemCounts.ContainsKey(item.itemName))
        {
            itemCounts[item.itemName]++;
        }
        else
        {
            itemCounts[item.itemName] = 1;
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].itemName.text == "������ ����ֽ��ϴ�")
                {
                    slots[i].InitializeSlot(item);
                    break;
                }
            }
        }
        UpdateItemCount(item.itemName);
        UpdateEmptyText();
        Debug.Log($"������ �߰� : {item.itemName}");
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

    public void EquipItem(Player player, ItemSO item)
    {
        player.EquipInventoryItem(item);
        Debug.Log($"������ ���� : {item.itemName}");
    }

    public bool UseItem(string itemName)
    {
        if (itemCounts.ContainsKey(itemName) && itemCounts[itemName] > 0)
        {
            itemCounts[itemName]--;
            UpdateItemCount(itemName);
            UpdateEmptyText();
            return true;
        }
        return false;
    }

    public void LoadData()
    {
        InvenData invenData = DataManager.Instance.LoadData<InvenData>();
        Debug.Log(invenData);

        if (invenData == null)
            return;
        //else
        //{
        //    for (int i = 0; i < invenData.itemList.Count; i++)
        //    {
        //        ItemSO itemData = DataManager.Instance.LoadItemSOData(invenData.itemList[i]);

        //        if (itemData != null)
        //        {
        //            AddItem(itemData);
        //        }
        //    }
        //}
        for (int i = 0; i < invenData.itemList.Count; i++)
        {
            SlotData slotData = invenData.itemList[i];
            ItemSO itemData = DataManager.Instance.LoadItemSOData(slotData);

            if (itemData != null)
            {
                //AddItem(itemData);
                //itemCounts[itemData.itemName] = slotData.itemCount;
                //UpdateItemCount(itemData.itemName);
                for (int j = 0; j < slots.Length; j++)
                {
                    if (slots[j].itemName.text == "������ ����ֽ��ϴ�")
                    {
                        slots[j].InitializeSlot(itemData);
                        itemCounts[itemData.itemName] = slotData.itemCount;
                        slots[j].UpdateItemCount(slotData.itemCount);
                        break;
                    }
                }
            }
        }
        UpdateEmptyText();
    }

    public void SaveData()
    {
        InvenData invenData = new InvenData();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].currentItem == null)
                continue;

            SlotData slotdata = new SlotData();
            slotdata.itemName = slots[i].currentItem.itemName;  // ���Կ� �ִ� ������ �̸�
            //int slotItemCount = itemCounts[name];
            slotdata.itemCount = itemCounts[slotdata.itemName];    // ���Կ� �ִ� ������ ����
            invenData.itemList.Add(slotdata);
        }

        //foreach (var slot in slots)
        //{
        //    if (slot.itemName.text != "������ ����ֽ��ϴ�")
        //    {
        //        SlotData slotData = new SlotData
        //        {
        //            itemName = slot.itemName.text,
        //            itemCount = itemCounts[slot.itemName.text]
        //        };
        //        invenData.itemList.Add(slotData);
        //    }
        //}

        DataManager.Instance.SaveData(invenData);
    }

    private void OnApplicationQuit()    // ������ ���� �� ����
    {
        SaveData();
    }
}

[Serializable]
public class InvenData    // ���Ե��� ������ �κ��丮
{
    public List<SlotData> itemList = new List<SlotData>();
}

[Serializable]
public class SlotData    // ���� �ϳ��� ������ ����
{
    public string itemName;
    public int itemCount;
}