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

    void Start()
    {
        if (equipButton != null)
        {
            equipButton.onClick.AddListener(ToggleEquip);
            // ������ư�� Ŭ�� �̺�Ʈ�� �߰�
        }
        // UpdateButton(); // ���⼭�� ������Ʈ�� ����ߵǳ�...
    }

    public void InitializeSlot(Sprite image, string name, string info)
    {
        itemImage.sprite = image;
        itemName.text = name;
        itemInfo.text = info;
        itemCount.text = "0";
        gameObject.SetActive(true); // ���� Ȱ��ȭ
        // Debug.Log($"Slot initialized: {name}, {info}");
    }

    public void ClearSlot()
    {
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
            UnequipItem(); // ������ ���� ȣ��
        }
    }

    void UpdateButton()
    {
        // ���� ���¿� ���� ���� / ���� ���� �ؽ�Ʈ �����
        equipButton.GetComponentInChildren<Text>().text = isEquipped ? "����" : "����";
    }

    void EquipItem()
    {
        // ������ ���� ���� �߰�
        if (characterStats != null)
        {
            characterStats.IncreaseStats(itemName.text);
        }
    }

    void UnequipItem()
    {
        // ������ ���� ���� �߰�
        if (characterStats != null)
        {
            characterStats.DecreaseStats(itemName.text);
        }
    }
}
