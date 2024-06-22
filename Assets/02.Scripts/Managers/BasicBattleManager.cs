using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBattleManager : MonoBehaviour
{
    public static BasicBattleManager Instance;

    public float enemySpawnRate = 1.0f;

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
        StartCoroutine(BasicBattle());
    }

    IEnumerator BasicBattle()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemySpawnRate);
        }
    }
}
