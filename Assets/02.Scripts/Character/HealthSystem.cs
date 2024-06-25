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

    public void TakeDamage(int damage)
    {
        if (health == 0) return;

        health = Mathf.Max(health - damage, 0);

        if(IsEnemy) // ���ʹ��� ��
        {
            OnTakeDamage?.Invoke(); // ������ ���� �� ��������
        }

        if (health == 0)
        {
            IsDie = true;
            OnDie?.Invoke();
        }
    }
}
