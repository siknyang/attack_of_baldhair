using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    [SerializeField]
    public float enemySpawnRate;

    public DungeonSO[] dungeonSOArray;

    private Dictionary<string,  DungeonSO> activeDungeons = new Dictionary<string, DungeonSO>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach(DungeonSO dungeonSO in dungeonSOArray)
        {
            activeDungeons[dungeonSO.name] = dungeonSO;
        }
    }

    public void EnterDungeon(string dungeonName)
    {
        if (activeDungeons.ContainsKey(dungeonName))
        {
            DungeonSO dungeonSO = activeDungeons[dungeonName];
            int enemyCount = CalculateEnemyCount(dungeonSO);
            StartCoroutine(DungeonBattle(dungeonSO, enemyCount));
        }
    }

    IEnumerator DungeonBattle(DungeonSO dungeonSO, int enemyCount)
    {
        int spawnedEnemies = 0;
        int diedEnemies = 0;

        while (spawnedEnemies < enemyCount)
        {
            yield return new WaitForSeconds(dungeonSO.dungeonLv);
            SpawnEnemy(dungeonSO.name);
            spawnedEnemies++;
        }
        while (diedEnemies < enemyCount)
        {
            yield return null;
        }

        DungeonCleared(dungeonSO.name);
    }

    private void SpawnEnemy(string dungeonName)
    {
        //TODO: 적 스폰 로직
        //TODO: 적 사망시 사망 메서드 호출
    }

    private void DungeonCleared(string dungeonName)
    {
        GiveReward(dungeonName);
    }

    private void GiveReward(string dungeonName)
    {
        if (activeDungeons.ContainsKey(dungeonName))
        {
            int reward = activeDungeons[dungeonName].reward;
            //TODO: 로직 추가
        }
    }

    private void LevelUPDungeon(string dungeonName)
    {
        if (activeDungeons.ContainsKey(dungeonName))
        {
            activeDungeons[dungeonName].dungeonLv++;
        }
    }

    private int CalculateEnemyCount(DungeonSO dungeonSO)
    {
        return dungeonSO.defaultEnemyNum + (dungeonSO.dungeonLv - 1) * 5;
    }
}
