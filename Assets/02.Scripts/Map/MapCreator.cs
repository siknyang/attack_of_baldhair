using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public GameObject unitPrefab;
    public int unitNum;   // ȭ�鿡 ǥ�õ� ���� ����
    private Queue<GameObject> activeUnit = new Queue<GameObject>();     // �� ���� ��� ť
    private float unitLength = 25f;   // ���� ����: �� ũ�⿡ ���� ���� �� ����
    private float spawnZ = 0.0f;   // ������ Z ��ġ

    protected void Start()
    {
        // ���� ī�޶� ã�Ƽ� ��ġ ����
        // ���� ī�޶� - �÷��̾� ����Ǹ� �÷��̾� �����ӿ��� ������
        GameObject cameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObj != null)
        {
            mainCamera = cameraObj.GetComponent<Camera>();
        }
        else
        {
            Debug.LogError("Camera not found");
            return;
        }

        InitializePool();
        ActiveFirst();
    }

    private void InitializePool()   // �� ���� �̸� ����
    {
        for (int i = 0; i < unitNum; i++)
        {
            GameObject instantiateUnit = Instantiate(unitPrefab, transform);    // ������ ���� ������ŭ ����
            instantiateUnit.SetActive(false);    // ��Ȱ��ȭ
            activeUnit.Enqueue(instantiateUnit);    // ť�� �ֱ�
        }
    }

    private void ActiveFirst()    // ���� �������� �� ��ġ ��ġ�� �ʰ� ��� Ȱ��ȭ
    {
        foreach (var units in activeUnit)
        {
            units.transform.position = new Vector3(0, -1f, spawnZ);
            units.SetActive(true);
            spawnZ += unitLength;
        }
    }

    void Update()
    {
        float cameraZ = mainCamera.transform.position.z;

        // ���� ����: ī�޶� �þ� + �� ���� ���� * 5 > ���� �� ���� ��ġ
        if (cameraZ + unitLength * 5 > spawnZ)     // �� ���� ���� ����Ǹ� ���� ����(unitNum - 1)
        {
            SpawnUnit();
        }
    }

    private void SpawnUnit()    // �� ���� Ȱ��ȭ
    {
        GameObject unitMap;
        unitMap = activeUnit.Dequeue();
        unitMap.transform.position = new Vector3(0, -1f, spawnZ);
        unitMap.SetActive(true);
        activeUnit.Enqueue(unitMap);
        spawnZ += unitLength;

        // ���� �޼���� ���� ������ ����
        // Ȱ��ȭ�� �� ������ �����ϸ� ������ �� �ڵ����� ��Ȱ��ȭ�Ͽ� �������� ���� �̿�,
        // Update �޼��忡�� ���� �������� ���� ������ ���������ν� ���� �޼��� ������ ��
    }
}
