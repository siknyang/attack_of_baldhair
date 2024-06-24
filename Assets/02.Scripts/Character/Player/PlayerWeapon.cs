using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private int damage;
    private float knockback;

    private float destroyTime = 3.0f; // ���⸦ ������ �ð�
    private bool hitEnemy = false; // ���� ������� ����

    private List<Collider> alreadyColliderWith = new List<Collider>();

    private void Awake()
    {
        myCollider = FindObjectOfType<Player>().GetComponent<CharacterController>();
        hitEnemy = false;
        StartCoroutine(DestroyWeaponAfterTime());
    }

    private void OnEnable()
    {
        alreadyColliderWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (alreadyColliderWith.Contains(other)) return;

        alreadyColliderWith.Add(other);

        if (other.TryGetComponent(out HealthSystem health))
        {
            health.TakeDamage(damage);
            hitEnemy = true; // ���� �������� ǥ��
        }

        if (other.TryGetComponent(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
            hitEnemy = true; // ���� �������� ǥ��
        }
    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }

    private IEnumerator DestroyWeaponAfterTime()
    {
        yield return new WaitForSeconds(destroyTime);

        if (!hitEnemy) // ���� ������ �ʾ��� ���
        {
            Destroy(gameObject); // ����
        }
    }
}
