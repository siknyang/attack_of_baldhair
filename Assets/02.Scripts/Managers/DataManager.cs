using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject("DataManager");
                    instance = gameObject.AddComponent<DataManager>();
                }
            }
            return instance;
        }
    }

    string savePath;

    private void Awake()
    {
        // �̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Application.persistentDataPath: ����ϰ� �ִ� ����� � ü���� ���� ���� ���� ��θ� �ڵ����� ã����
        savePath = Application.persistentDataPath;
    }

    public void SaveData<T>(T json)     // ���׸� Ÿ���� ������ ����
    {
        File.WriteAllText(savePath + $"/{typeof(T).ToString()}.txt", JsonUtility.ToJson(json));     // JSON���� ����ȭ�Ͽ� Ÿ�� ������ ���� ����
        Debug.Log("���� �Ϸ�: " + savePath + $"/{typeof(T).ToString()}.txt");
    }

    public T LoadData<T>()    // ���׸� Ÿ���� ������ �ҷ�����
    {
        if (!File.Exists(savePath + $"/{typeof(T).ToString()}.txt"))    // ��ο� ������ ���� �� ���� ó��
            return JsonUtility.FromJson<T>(null);

        string jsonData = File.ReadAllText(savePath + $"/{typeof(T).ToString()}.txt");
        Debug.Log("�ҷ����� �Ϸ�: " + savePath + $"/{typeof(T).ToString()}.txt");
        return JsonUtility.FromJson<T>(jsonData);
    }

    public ItemSO LoadItemSOData(SlotData data)
    {
        // Resources ������ �ִ� ScriptableObject/��ο��� itemName�� SO�� �ҷ���
        return Resources.Load<ItemSO>("ScriptableObject/Data/" + data.itemName);
    }
}

// TODO: InvenData, SlotData �κ��丮 Ŭ������ �ű��
// �����Ϸ��� ������ ����
[Serializable]
public class InvenData    // ���Ե��� ������ �κ��丮
{
    public List<SlotData> itemList = new List<SlotData>();
}

[Serializable]
public class SlotData    // ���� �ϳ��� ������ ����
{
    public string itemName;
    public int itemCount;
}