using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour, IRandomPosition
{
    public ObjectPoolManager objectPoolManager;

    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
        //int poolSize = objectPoolManager.GetPoolSize("Enemy");
    }

    private void Update()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = GetRandomPosition();
        objectPoolManager.SpawnFromPool("Enemy", spawnPos);
    }

    public Vector3 GetRandomPosition()
    {
        float x = Random.Range(-14.0f, 14.0f);
        float y = 0;
        float z = Random.Range(0.0f, 10.0f);

        return new Vector3(x, y, z);
    }
}
