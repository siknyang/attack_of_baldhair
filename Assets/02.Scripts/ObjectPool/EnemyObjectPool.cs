using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour, IRandomPosition
{
    public ObjectPoolManager objectPoolManager;
    public int poolSize;

    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
        poolSize = objectPoolManager.GetPoolSize("Enemy");
        StartCoroutine(SpawnEnemy(poolSize));
    }   

    IEnumerator SpawnEnemy(int poolSize)
    {
            for (int i =0 ; i < poolSize; i++)
            {
                Vector3 spawnPos = GetRandomPosition();
                objectPoolManager.SpawnFromPool("Enemy", spawnPos);
            }

            yield return new WaitForSeconds(1.0f);

    }

    public Vector3 GetRandomPosition()
    {
        // TODO: x, z 범위 두 군데 설정하기
        float x = Random.Range(-12.0f, -7.0f);
        float y = 0;
        float z = Random.Range(0.0f, 10.0f);

        return new Vector3(x, y, z);
    }
}
