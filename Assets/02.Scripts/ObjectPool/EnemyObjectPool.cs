using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObjectPool : MonoBehaviour, IRandomPosition
{
    public ObjectPoolManager objectPoolManager;
    private int poolSize;
    private ObjectPoolManager.Pool pool;

    private void Start()
    {
        objectPoolManager = ObjectPoolManager.Instance;
        pool = objectPoolManager.GetPool("Enemy");
        poolSize = pool.poolSize;
        ActiveFirst();
        StartCoroutine(SpawnEnemy());
    }   

    private void ActiveFirst()    // 게임 실행했을 때 한 번에 다 생성
    {
        if (pool.tag == "Enemy")
        {
            foreach (var obj in objectPoolManager.poolDictionary[pool.tag])
            {
                obj.transform.position = GetRandomPosition();
                obj.SetActive(true);
            }
        }
    }

    public IEnumerator SpawnEnemy()
    {
        while (true)
        {
            int activeEnemy = GetActiveEnemyNum();
            if (activeEnemy < pool.poolSize)
            {
                Vector3 spawnPos = GetRandomPosition();
                objectPoolManager.SpawnFromPool("Enemy");
            }

            yield return new WaitForSeconds(0.1f);
        }    
    }

    private int GetActiveEnemyNum()
    {
        int num = 0;
        if (pool.tag == "Enemy")
        {
            foreach (var obj in objectPoolManager.poolDictionary[pool.tag])
            {
                if (obj.activeSelf)
                    num++;
            }
        }
        return num;
    }

    public Vector3 GetRandomPosition()
    {
        // TODO: x, z 범위 두 군데 설정하기
        //float x;
        //if (Random.value < 0.5f)
        //    x = Random.Range(-14.0f, -7f);
        //else
        //    x = Random.Range(7.0f, 14f); ;

        //float y = 0;

        //float z;
        //if (Random.value < 0.5f)
        //    z = Random.Range(-14.0f, -7f);
        //else
        //    z = Random.Range(7.0f, 14f); ;
        float x = Random.Range(-14.0f, 14f);
        float y = 0;
        float z = Random.Range(-14.0f, 14f);

        return new Vector3(x, y, z);
    }
}
