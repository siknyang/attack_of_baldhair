using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    string savePath;

    protected override void Awake()
    {
        base.Awake();

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