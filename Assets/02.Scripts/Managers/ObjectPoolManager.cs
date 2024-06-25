using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IRandomPosition     // 계속해서 스폰
{
    Vector3 GetRandomPosition();
}

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public string tag;
        public int poolSize;
        public Transform spawnPos;
        public LayerMask collisionLayer;    // 어느 레이어에 충돌하면 사라지게 할지
    }

    public List<Pool> pools = new List<Pool>();
    Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        Instance.InitializePool();
    }

    public void InitializePool()
    {
        foreach (var pool in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.spawnPos);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 pos)
    {
        if (!poolDictionary.ContainsKey(tag))   // 태그가 일치하는 오브젝트를 탐색
        {
            Debug.LogWarning($"{tag} 태그를 가진 풀이 없습니다.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();    // 사용하려고 큐에서 뺌
        obj.transform.position = pos;
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj);    // 사용이 끝나서 큐에 다시 넣음
        return obj;
    }

    // 봐서 삭제
    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))   // 태그가 일치하는 오브젝트를 탐색
        {
            Debug.LogWarning($"{tag} 태그를 가진 풀이 없습니다.");
            return;
        }

        obj.SetActive(false);
    }

    public int GetPoolSize(string tag)
    {
        foreach(var pool in pools)
        {
            if (pool.tag == tag)
                return pool.poolSize;
        }
        Debug.LogWarning($"{tag} 태그를 가진 풀이 없습니다.");
        return 0;
    }

    public Pool GetPool(string tag)
    {
        foreach (var pool in pools)
        {
            if (pool.tag == tag)
                return pool;
        }
        Debug.LogWarning($"{tag} 태그를 가진 풀이 없습니다.");
        return null;
    }
}
