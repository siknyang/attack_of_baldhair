using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager1 : ObjectPoolManager
{
    [SerializeField] private Camera mainCamera;
    private float unitLength = 25f;   // 유닛 길이: 맵 크기에 따라 변할 수 있음
    private float spawnZ = 0.0f;   // 생성될 Z 위치

    protected override void Start()
    {
        // 메인 카메라 찾아서 위치 참조
        // 메인 카메라 - 플레이어 연결되면 플레이어 움직임에도 반응함
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

    private void ActiveFirst()    // 게임 시작했을 때 위치 겹치지 않게 모두 활성화
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

        // 생성 조건: 카메라 시야 + 맵 유닛 길이 * 9 > 현재 맵 유닛 위치
        if (cameraZ + unitLength * 14 > spawnZ)     // 맵 유닛 개수 변경되면 숫자 수정
        {
            SpawnUnit();
        }
    }

    private void SpawnUnit()    // 맵 유닛 활성화
    {
        GameObject unitMap = SpawnFromPool("Map", new Vector3(0, -1f, spawnZ));
        if (unitMap != null)
            spawnZ += unitLength;

        // 제거 메서드는 따로 만들지 않음
        // 활성화할 때 개수가 부족하면 오래된 걸 자동으로 비활성화하여 가져오는 점을 이용,
        // Update 메서드에서 생성 조건으로 제거 범위를 조정함으로써 제거 메서드 역할을 함
    }
}
