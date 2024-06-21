using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Camera camera;
    public GameObject unitPrefab;
    public int unitNum; // ȭ�鿡 ǥ�õ� ���� ����
    private Queue<GameObject> activeUnit = new Queue<GameObject>();
    private float unitLength = 31.3f;   // ���� ����
    private float spawnZ = 15.0f;   // ������ Z ��ġ

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

        // ���� ����
        if (cameraZ + 8 > usedUnit.transform.position.z + unitLength)
        {
            SpawnUnit();
        }

        // ���� ����(���� ���� ����� ������ ������ �� ������ ���� �����ٰ� ���� �Ŷ� �ش� �ڵ� ���۵�. ������ ���ܰ� ���� �� �����Ƿ� �ϴ� ����)
        if (usedUnit.transform.position.z + unitLength < camera.transform.position.z)
        {
            RemoveUnit();
        }
    }

    private void SpawnUnit()
    {
        GameObject unitMap;
        if (activeUnit.Count < unitNum)     // ����
        {
            unitMap = Instantiate(unitPrefab, new Vector3(50, 1.4f, spawnZ), Quaternion.identity);
            activeUnit.Enqueue(unitMap);
        }
        else    // Ȱ��ȭ
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
