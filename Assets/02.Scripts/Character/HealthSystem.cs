using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 200;
    private int health;
    public event Action OnDie;
    public event Action OnTakeDamage;
    public bool IsEnemy { get; set; }

    public bool IsDie = false;
    [SerializeField] private GameObject hitTextPrefab;

    private void Start()
    {
        health = maxHealth;
        IsDie = false;
    }

    public void ResetHealth()    // 적 재생성 시 체력 초기화
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (health == 0) return;

        health = Mathf.Max(health - damage, 0);

        if(IsEnemy) // 에너미일 때
        {
            OnTakeDamage?.Invoke(); // 데미지 받을 때 깜빡깜빡
            ShowHitText();
        }

        if (health == 0)
        {
            IsDie = true;
            OnDie?.Invoke();
        }
    }

    private void ShowHitText()
    {
        if (hitTextPrefab != null)
        {
            // 에너미 머리 위에 텍스트 생성
            Vector3 spawnPosition = transform.position + Vector3.up * 2;
            GameObject hitText = Instantiate(hitTextPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
