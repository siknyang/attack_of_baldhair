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

    public void SaveData<T>(T json)     // 제네릭 타입의 데이터 저장
    {
        File.WriteAllText(savePath + $"/{typeof(T).ToString()}.txt", JsonUtility.ToJson(json));     // 
        Debug.Log("저장 완료: " + savePath + "/PlayerData.txt");
    }

    public void SavePlayerData(string json)    // 플레이어 데이터 저장
    {
        File.WriteAllText(savePath + "/PlayerData.txt", json);
        Debug.Log("저장 완료: " +  savePath + "/PlayerData.txt");
    }

    //public PlayerData LoadPlayerData()    // 플레이어 데이터 불러오기
    //{
    //    if (!File.Exists(savePath + "/PlayerData.txt"))    // 경로에 데이터 없을 때 예외 처리
    //        return null;
        
    //    string jsonData = File.ReadAllText(savePath + "/PlayerData.txt");
    //    return JsonUtility.FromJson<PlayerData>(jsonData);
    //}

    public void SaveInvenData(string json)    // 인벤토리 데이터 저장
    {
        File.WriteAllText(savePath + "/InvenData.txt", json);
        Debug.Log("저장 완료: " + savePath + "/InvenData.txt");
    }

    public InvenData LoadInvenData()    // 인벤토리 데이터 불러오기
    {
        if (!File.Exists(savePath + "/InvenData.txt"))    // 경로에 데이터 없을 때 예외 처리
            return null;

        string jsonData = File.ReadAllText(savePath + "/InvenData.txt");
        return JsonUtility.FromJson<InvenData>(jsonData);
    }

    // 아이템 SO 스크립트 만들어지면 주석 풀기
    // 아이템 SO 스크립트 이름 ItemData, 아니면 바꾸기
    //public ItemData LoadItemSOData(SlotData data)
    //{
    //    // Resources 폴더에 있는 ScriptableObject/경로에서 itemName인 SO를 불러옴
    //    return Resources.Load<ItemData>("ScriptableObject/경로" + data.itemName);
    //}
}

// 저장하려는 데이터 묶음
[Serializable]
public class InvenData    // 슬롯들의 모음인 인벤토리
{
    public List<SlotData> itemList = new List<SlotData>();
}

[Serializable]
public class SlotData    // 슬롯 하나의 아이템 정보
{
    public string itemName;
    public int itemCount;
}