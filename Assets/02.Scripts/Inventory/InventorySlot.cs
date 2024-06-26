using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // ���� 1ĭ �ȿ��ִ� ���(�̹���,�̸�,����)�� �����ϴ� Ŭ���� + ����/���� ���

    public Image itemImage;
    public Text itemName;
    public Text itemInfo;
    public Text itemCount; // ������ ����ǥ�� �߰���
    public Button equipButton;
    private bool isEquipped = false; // ���� ������ ���� ����
    // false = ������ ����, true = ������ ����

    public CharacterStats characterStats; // ĳ���� ���� Ŭ����

    public ItemSO currentItem;

    void Start()
    {
        if (equipButton != null)
        {
            equipButton.onClick.AddListener(ToggleEquip);
            // ������ư�� Ŭ�� �̺�Ʈ�� �߰�
        }
    }

    public void InitializeSlot(ItemSO item)
    {
        currentItem = item;
        itemImage.sprite = item.icon;
        itemName.text = item.itemName;
        itemInfo.text = item.itemDescription;
        itemCount.text = "0";
        gameObject.SetActive(true); // ���� Ȱ��ȭ
        // Debug.Log($"Slot initialized: {name}, {info}");
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemName.text = "������ ����ֽ��ϴ�";
        itemInfo.text = "�������� �����ϼ���";
        itemCount.text = "0";
        gameObject.SetActive(true);
    }

    public void UpdateItemCount(int count)
    {
        itemCount.text = count.ToString();
    }

    void ToggleEquip()
    {
        isEquipped = !isEquipped; // �������� ������Ű��
        UpdateButton();

        if (isEquipped ) // false �϶�
        {
            EquipItem(); // ������ ���� ȣ��
        }
        else // true �϶�
        {
            UnEquipItem(); // ������ ���� ȣ��
        }
    }

    void UpdateButton()
    {
        // ���� ���¿� ���� ���� / ���� ���� �ؽ�Ʈ �����
        equipButton.GetComponentInChildren<Text>().text = isEquipped ? "����" : "����";
    }

    void EquipItem()
    {
        if (characterStats != null && currentItem != null)
        {
            switch (currentItem.itemName)
            {
                case "Ʈ��Ʈ��Ʈ":
                    characterStats.IncreaseAttackSpeed(5.0f);
                    break;
                case "��":
                    characterStats.IncreaseAttackPower(10.0f);
                    break;
                default:
                    break;
            }
            Debug.Log($"������ ���� : {currentItem.itemName}");
        }
    }

    void UnEquipItem()
    {
        if (characterStats != null && currentItem != null)
        {
            switch (currentItem.itemName)
            {
                case "Ʈ��Ʈ��Ʈ":
                    characterStats.IncreaseAttackSpeed(-5.0f);
                    break;
                case "��":
                    characterStats.IncreaseAttackPower(-10.0f);
                    break;
                default:
                    break;
            }
            Debug.Log($"������ ���� : {currentItem.itemName}");
        }
    }

    //void EquipItem()
    //{
    //    // ������ ���� ���� �߰�
    //    if (characterStats != null && currentItem != null)
    //    {
    //        characterStats.IncreaseStats(currentItem.itemName);
    //        Debug.Log($"������ ���� : {currentItem.itemName}");
    //    }
    //}

    //void UnequipItem()
    //{
    //    // ������ ���� ���� �߰�
    //    if (characterStats != null && currentItem != null)
    //    {
    //        characterStats.DecreaseStats(currentItem.itemName);
    //        Debug.Log($"������ ���� : {currentItem.itemName}");
    //    }
    //}
}
