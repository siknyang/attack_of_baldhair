using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public GameObject unitPrefab;
    public int unitNum;   // 화면에 표시될 유닛 개수
    private Queue<GameObject> activeUnit = new Queue<GameObject>();     // 맵 유닛 담길 큐
    private float unitLength = 25f;   // 유닛 길이: 맵 크기에 따라 변할 수 있음
    private float spawnZ = 0.0f;   // 생성될 Z 위치

    protected void Start()
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

        InitializePool();
        ActiveFirst();
    }

    private void InitializePool()   // 맵 유닛 미리 생성
    {
        for (int i = 0; i < unitNum; i++)
        {
            GameObject instantiateUnit = Instantiate(unitPrefab, transform);    // 설정한 유닛 개수만큼 생성
            instantiateUnit.SetActive(false);    // 비활성화
            activeUnit.Enqueue(instantiateUnit);    // 큐에 넣기
        }
    }

    private void ActiveFirst()    // 게임 시작했을 때 위치 겹치지 않게 모두 활성화
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

        // 생성 조건: 카메라 시야 + 맵 유닛 길이 * 5 > 현재 맵 유닛 위치
        if (cameraZ + unitLength * 5 > spawnZ)     // 맵 유닛 개수 변경되면 숫자 수정(unitNum - 1)
        {
            SpawnUnit();
        }
    }

    private void SpawnUnit()    // 맵 유닛 활성화
    {
        GameObject unitMap;
        unitMap = activeUnit.Dequeue();
        unitMap.transform.position = new Vector3(0, -1f, spawnZ);
        unitMap.SetActive(true);
        activeUnit.Enqueue(unitMap);
        spawnZ += unitLength;

        // 제거 메서드는 따로 만들지 않음
        // 활성화할 때 개수가 부족하면 오래된 걸 자동으로 비활성화하여 가져오는 점을 이용,
        // Update 메서드에서 생성 조건으로 제거 범위를 조정함으로써 제거 메서드 역할을 함
    }
}
