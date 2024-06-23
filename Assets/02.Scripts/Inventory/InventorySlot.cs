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

    public Button equipButton;
    private bool isEquipped = false; // ���� ������ ���� ����
    // false = ������ ����, true = ������ ����

    void Start()
    {
        equipButton.onClick.AddListener(ToggleEquip);
        // ������ư�� Ŭ�� �̺�Ʈ�� �߰�

        // UpdateButton(); // ���⼭�� ������Ʈ�� ����ߵǳ�...
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
    }

    void UnequipItem()
    {
        // ������ ���� ���� �߰�
    }
}
