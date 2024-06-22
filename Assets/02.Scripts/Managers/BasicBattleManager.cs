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

    private void StartBattle(int enemyCount)
    {
        StartCoroutine(BasicBattle(enemyCount));
    }

    IEnumerator BasicBattle(int enemyCount)
    {
        List<Enemy> enemies = SpawnEnemies(enemyCount);

        while (true)
        {
            if (player.health <= 0 && !isPlayerDead)
            {
                isPlayerDead = true;
                StartCoroutine(RespawnPlayer());
            }

            yield return null;

            enemies = enemies.FindAll(enemy => enemy.health != null && enemy.health > 0);

            if (enemies.Count == 0)
            {
                enemies = SpawnEnemies(enemyCount);
            }
        }

        if (player.health <= 0)
        {
            //부활 스폰 로직
        }
    }

    private List<Enemy> SpawnEnemies(int enemyCount)
    {
        List<Enemy> enemies = new List<Enemy>();

        for (int i = 0; i < enemyCount; i++)
        {
            Transform spawnPoint = enemySpawnPoints[UnityEngine.Random.Range(0, enemySpawnPoints.Length)]; //Random.Range가 그냥 호출은 되지 않아서 앞에 UnityEngine 추가
            GameObject enemyGO = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            enemies.Add(enemy);
        }

        return enemies;
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(2);
        GameObject playerGO = Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
        Initialize(player);
        isPlayerDead = false;
    }
}
