using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInfinite     // ����ؼ� ����
{
    IEnumerator SpawnObject();
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

    [SerializeField] protected List<Pool> pools = new List<Pool>();
    protected Dictionary<string, Queue<GameObject>> poolDictionary;

    protected virtual void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        Instance.InitializePool();
    }

    public virtual void InitializePool()
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

    public virtual GameObject SpawnFromPool(string tag, Vector3 pos)
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

    public virtual void ReturnToPool(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))   // �±װ� ��ġ�ϴ� ������Ʈ�� Ž��
        {
            Debug.LogWarning($"{tag} �±׸� ���� Ǯ�� �����ϴ�.");
            return;
        }

        obj.SetActive(false);
    }
}
