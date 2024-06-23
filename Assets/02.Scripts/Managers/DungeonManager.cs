using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public static DungeonManager Instance;

    public GameObject gameClearPanel;
    public GameObject gameOverPanel;

    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;
    public GameObject enemyPrefab;

    public DungeonSO[] dungeonSOArray;
    private Dictionary<string,  DungeonSO> activeDungeons = new Dictionary<string, DungeonSO>();

    private Player player;

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

    public void Initialize(Player player)
    {
        this.player = player;
    }

    public void EnterDungeon(string dungeonName)
    {
        if (activeDungeons.ContainsKey(dungeonName))
        {
            DungeonSO dungeonSO = activeDungeons[dungeonName];
            StartCoroutine(DungeonBattle(dungeonSO));
        }
    }

    IEnumerator DungeonBattle(DungeonSO dungeonSO)
    {
        List<Enemy> enemies = SpawnEnemy(CalculateEnemyCount(dungeonSO));

        while (player.health > 0 && enemies.Count > 0)
        {
            yield return null;
            enemies = enemies.FindAll(enemy => enemy != null && enemy.health > 0);
        }

        if(player.health > 0)
        {
            DungeonCleared(dungeonSO);
            gameClearPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(true);
        }


    }

    private List<Enemy> SpawnEnemy(int enemyCount)
    {
        List<Enemy> enemies = new List<Enemy>();

        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length)]; //Random.Range가 그냥 호출은 되지 않아서 앞에 UnityEngine 추가
            GameObject enemyGO = Instantiate(enemyPrefab); //이름 변경 가능성 O, 아무튼 적 프리팹 불러내기 필요
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            enemies.Add(enemy);
        }
        
        return enemies;
    }

    private void DungeonCleared(DungeonSO dungeonSO)
    {
        GiveReward(dungeonSO);
        LevelUPDungeon(dungeonSO.name);
    }

    private void GiveReward(DungeonSO dungeonSO)
    {
        switch(dungeonSO.Type)
        {
            case DungeonType.Coin:
                player.coin += dungeonSO.reward;
                break;
            case DungeonType.Exp:
                player.experience += dungeonSO.reward;
                break;
            case DungeonType.Random: //지피티의 도움을 받았습니다...
                if(UnityEngine.Random.value > 0.5f)
                {
                    player.coin += dungeonSO.reward;
                }
                else
                {
                    player.experience += dungeonSO.reward;
                }
                break;
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
