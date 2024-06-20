using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    public GameObject unitPrefab;
    public int unitNum; // 화면에 표시될 유닛 개수
    private Queue<GameObject> activeUnit = new Queue<GameObject>();
    private float unitLength = 31.3f;   // 유닛 길이
    private float spawnZ = 15.0f;   // 생성될 Z 위치

    void Start()
    {
        camera = GetComponent<Camera>();

        for (int i = 0; i < unitNum; i++)
        {
            SpawnUnit();
        }
    }

    void Update()
    {
        GameObject usedUnit = activeUnit.Peek();
        float cameraZ = camera.transform.position.z;

        // 생성 조건
        if (cameraZ + 8 > usedUnit.transform.position.z + unitLength)
        {
            SpawnUnit();
        }

        // 제거 조건(현재 제거 방식이 개수가 부족할 때 오래된 것을 가져다가 쓰는 거라서 해당 코드 미작동. 하지만 예외가 있을 수 있으므로 일단 구현)
        if (usedUnit.transform.position.z + unitLength < camera.transform.position.z)
        {
            RemoveUnit();
        }
    }

    private void SpawnUnit()
    {
        GameObject unitMap;
        if (activeUnit.Count < unitNum)     // 생성
        {
            unitMap = Instantiate(unitPrefab, new Vector3(50, 1.4f, spawnZ), Quaternion.identity);
            activeUnit.Enqueue(unitMap);
        }
        else    // 활성화
        {
            unitMap = activeUnit.Dequeue();
            unitMap.transform.position = new Vector3(50, 1.4f, spawnZ);
            unitMap.SetActive(true);
            activeUnit.Enqueue(unitMap);
        }
        spawnZ += unitLength;
    }

    private void RemoveUnit()
    {
        GameObject usedUnit = activeUnit.Peek();
        if (usedUnit.transform.position.z + unitLength < camera.transform.position.z)
        {
            usedUnit.SetActive(false);
            activeUnit.Dequeue();
        }
    }
}
