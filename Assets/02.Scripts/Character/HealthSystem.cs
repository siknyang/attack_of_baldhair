using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int health;
    public event Action OnDie;
    public event Action OnTakeDamage;
    public bool IsEnemy { get; set; }

    public bool IsDie = false;

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
        }

        if (health == 0)
        {
            IsDie = true;
            OnDie?.Invoke();
        }
    }
}
