using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBattleManager : MonoBehaviour
{
    public static BasicBattleManager Instance;
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPoints;

    private Player player;
    private bool isPlayerDead = false;
    private int initialEnemyCount = 5;

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

    public void Initialize(Player player)
    {
        this.player = player;
    }

    private void Start()
    {
        StartBattle(initialEnemyCount);
    }

    public void StartBattle(int enemyCount)
    {
        GameObject playerGO = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        player = playerGO.GetComponent<Player>();
        StartCoroutine(BasicBattle(enemyCount));
    }

    IEnumerator BasicBattle(int enemyCount)
    {
        SpawnEnemies(enemyCount);

        while (true)
        {
            if (player.health <= 0 && !isPlayerDead)
            {
                isPlayerDead = true;
                StartCoroutine(RespawnPlayer());
            }

            yield return null;

            yield return new WaitForSeconds(1);
            SpawnEnemies(2);
        }
    }

    private void SpawnEnemies(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length)]; //Random.Range가 그냥 호출은 되지 않아서 앞에 UnityEngine 추가
            GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Enemy enemy = enemyGO.GetComponent<Enemy>();
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2);
        GameObject playerGO = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        player = playerGO.GetComponent<Player>();
        Initialize(player);
        isPlayerDead = false;
    }
}
