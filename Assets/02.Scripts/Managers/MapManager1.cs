using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager1 : ObjectPoolManager
{
    [SerializeField] private Camera mainCamera;
    private float unitLength = 25f;   // ���� ����: �� ũ�⿡ ���� ���� �� ����
    private float spawnZ = 0.0f;   // ������ Z ��ġ

    protected override void Start()
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

        base.Start();
        ActiveFirst();
    }

    private void ActiveFirst()    // ���� �������� �� ��ġ ��ġ�� �ʰ� ��� Ȱ��ȭ
    {
        foreach (var pool in pools)
        {
            if (pool.tag == "Map")
            {
                for (int i = 0; i < pool.poolSize; i++)
                {
                    GameObject obj = poolDictionary[pool.tag].Dequeue();
                    obj.transform.position = new Vector3(0, -1f, spawnZ);
                    obj.SetActive(true);
                    spawnZ += unitLength;
                    poolDictionary[pool.tag].Enqueue(obj);
                }
            

            }
        }
    }

    void Update()
    {
        float cameraZ = mainCamera.transform.position.z;

        // ���� ����: ī�޶� �þ� + �� ���� ���� * 9 > ���� �� ���� ��ġ
        if (cameraZ + unitLength * 14 > spawnZ)     // �� ���� ���� ����Ǹ� ���� ����
        {
            SpawnUnit();
        }
    }

    private void SpawnUnit()    // �� ���� Ȱ��ȭ
    {
        GameObject unitMap = SpawnFromPool("Map", new Vector3(0, -1f, spawnZ));
        if (unitMap != null)
            spawnZ += unitLength;

        // ���� �޼���� ���� ������ ����
        // Ȱ��ȭ�� �� ������ �����ϸ� ������ �� �ڵ����� ��Ȱ��ȭ�Ͽ� �������� ���� �̿�,
        // Update �޼��忡�� ���� �������� ���� ������ ���������ν� ���� �޼��� ������ ��
    }
}
