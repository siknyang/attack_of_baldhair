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
        // 싱글톤
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

        // Application.persistentDataPath: 사용하고 있는 기기의 운영 체제에 따른 적정 저장 경로를 자동으로 찾아줌
        savePath = Application.persistentDataPath;
    }

    public void SaveData<T>(T json)     // 제네릭 타입의 데이터 저장
    {
        File.WriteAllText(savePath + $"/{typeof(T).ToString()}.txt", JsonUtility.ToJson(json));     // JSON으로 직렬화하여 타입 명으로 파일 저장
        Debug.Log("저장 완료: " + savePath + $"/{typeof(T).ToString()}.txt");
    }

    public T LoadData<T>()    // 제네릭 타입의 데이터 불러오기
    {
        if (!File.Exists(savePath + $"/{typeof(T).ToString()}.txt"))    // 경로에 데이터 없을 때 예외 처리
            return JsonUtility.FromJson<T>(null);

        string jsonData = File.ReadAllText(savePath + $"/{typeof(T).ToString()}.txt");
        Debug.Log("불러오기 완료: " + savePath + $"/{typeof(T).ToString()}.txt");
        return JsonUtility.FromJson<T>(jsonData);
    }

    public ItemSO LoadItemSOData(SlotData data)
    {
        // Resources 폴더에 있는 ScriptableObject/경로에서 itemName인 SO를 불러옴
        return Resources.Load<ItemSO>("ScriptableObject/Data/" + data.itemName);
    }
}

// TODO: InvenData, SlotData 인벤토리 클래스로 옮기기
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