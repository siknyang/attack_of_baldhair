using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    string savePath;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.persistentDataPath;
    }

    public void SaveData<T>(T json)     // ���׸� Ÿ���� ������ ����
    {
        File.WriteAllText(savePath + $"/{typeof(T).ToString()}.txt", JsonUtility.ToJson(json));     // 
        Debug.Log("���� �Ϸ�: " + savePath + "/PlayerData.txt");
    }

    public void SavePlayerData(string json)    // �÷��̾� ������ ����
    {
        File.WriteAllText(savePath + "/PlayerData.txt", json);
        Debug.Log("���� �Ϸ�: " +  savePath + "/PlayerData.txt");
    }

    //public PlayerData LoadPlayerData()    // �÷��̾� ������ �ҷ�����
    //{
    //    if (!File.Exists(savePath + "/PlayerData.txt"))    // ��ο� ������ ���� �� ���� ó��
    //        return null;
        
    //    string jsonData = File.ReadAllText(savePath + "/PlayerData.txt");
    //    return JsonUtility.FromJson<PlayerData>(jsonData);
    //}

    public void SaveInvenData(string json)    // �κ��丮 ������ ����
    {
        File.WriteAllText(savePath + "/InvenData.txt", json);
        Debug.Log("���� �Ϸ�: " + savePath + "/InvenData.txt");
    }

    public InvenData LoadInvenData()    // �κ��丮 ������ �ҷ�����
    {
        if (!File.Exists(savePath + "/InvenData.txt"))    // ��ο� ������ ���� �� ���� ó��
            return null;

        string jsonData = File.ReadAllText(savePath + "/InvenData.txt");
        return JsonUtility.FromJson<InvenData>(jsonData);
    }

    // ������ SO ��ũ��Ʈ ��������� �ּ� Ǯ��
    // ������ SO ��ũ��Ʈ �̸� ItemData, �ƴϸ� �ٲٱ�
    //public ItemData LoadItemSOData(SlotData data)
    //{
    //    // Resources ������ �ִ� ScriptableObject/��ο��� itemName�� SO�� �ҷ���
    //    return Resources.Load<ItemData>("ScriptableObject/���" + data.itemName);
    //}
}

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