using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IRandomPosition
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
    }

    public List<Pool> pools = new List<Pool>();
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    protected override void Awake()
    {
        base.Awake();
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
                GameObject obj = Instantiate(pool.prefab, transform.parent);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objPool);
        }
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))   // �±װ� ��ġ�ϴ� ������Ʈ�� Ž��
        {
            Debug.LogWarning($"{tag} �±׸� ���� Ǯ�� �����ϴ�.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();    // ����Ϸ��� ť���� ��
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj);    // ����� ������ ť�� �ٽ� ����
        return obj;
    }

    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))   // �±װ� ��ġ�ϴ� ������Ʈ�� Ž��
        {
            Debug.LogWarning($"{tag} �±׸� ���� Ǯ�� �����ϴ�.");
            return;
        }

        obj.SetActive(false);
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
