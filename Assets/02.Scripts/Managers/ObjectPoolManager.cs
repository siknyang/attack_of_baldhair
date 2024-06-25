using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IRandomPosition     // ����ؼ� ����
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
        public LayerMask collisionLayer;    // ��� ���̾ �浹�ϸ� ������� ����
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
        if (!poolDictionary.ContainsKey(tag))   // �±װ� ��ġ�ϴ� ������Ʈ�� Ž��
        {
            Debug.LogWarning($"{tag} �±׸� ���� Ǯ�� �����ϴ�.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();    // ����Ϸ��� ť���� ��
        obj.transform.position = pos;
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj);    // ����� ������ ť�� �ٽ� ����
        return obj;
    }

    // ���� ����
    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))   // �±װ� ��ġ�ϴ� ������Ʈ�� Ž��
        {
            Debug.LogWarning($"{tag} �±׸� ���� Ǯ�� �����ϴ�.");
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
        Debug.LogWarning($"{tag} �±׸� ���� Ǯ�� �����ϴ�.");
        return 0;
    }

    public Pool GetPool(string tag)
    {
        foreach (var pool in pools)
        {
            if (pool.tag == tag)
                return pool;
        }
        Debug.LogWarning($"{tag} �±׸� ���� Ǯ�� �����ϴ�.");
        return null;
    }
}
