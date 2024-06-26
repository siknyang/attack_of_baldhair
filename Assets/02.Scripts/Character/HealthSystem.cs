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

    public void ResetHealth()    // �� ����� �� ü�� �ʱ�ȭ
    {
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (health == 0) return;

        health = Mathf.Max(health - damage, 0);

        if(IsEnemy) // ���ʹ��� ��
        {
            OnTakeDamage?.Invoke(); // ������ ���� �� ��������
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
            // ���ʹ� �Ӹ� ���� �ؽ�Ʈ ����
            Vector3 spawnPosition = transform.position + Vector3.up * 2;
            GameObject hitText = Instantiate(hitTextPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
