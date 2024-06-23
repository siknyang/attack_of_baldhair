using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // ���� �����ϴ� ��. �� ���Կ� ������ �ֱ�

    public InventorySlot[] slots;   // �������� �κ��丮 ������ �����ϴ� �迭
    public Sprite[] itemImages;     // ������ �̹��� �迭
    public string[] itemNames;      // ������ �̸� �迭
    public string[] itemInfo;       // ������ ���� �迭

    void Start()
    {  
        InitializeSlots();
    }

    void InitializeSlots() // ���� �ʱ�ȭ �޼���
    {
        // �迭�� ��ȸ�ϸ鼭 �� ���Կ� ������ �̹��� ����
        for (int i = 0; i < slots.Length; i++) // i = �ε��� ����. �ʱⰪ 0; slot �迭�� ���̺��� ������������ �ݺ�; �� �ݺ����� 1�� ����.
        {
            if (i < itemImages.Length) // �迭�� ���̺��� �۴ٸ�
            {
                // slots[i] �� slots �迭�� i��°�� �ǹ�
                slots[i].itemImage.sprite = itemImages[i]; // itemImage.sprite�� �̹��� ����
                slots[i].itemName.text = itemNames[i]; // itemName.text�� �̸� ����
                slots[i].itemInfo.text = itemInfo[i]; // itemInfo.text�� ���� ����
                slots[i].gameObject.SetActive(true); // ������ gameObject Ȱ��ȭ
            }
            else // i�� itemImages �迭�� ���̺��� ũ�ų� ������
            {
                slots[i].gameObject.SetActive(false); // ������ ���ٸ� ���� ���� ��Ȱ��ȭ
            }
        }
    }

    // �������� �κ��丮�� �߰��ϴ� �޼���
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
