using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    public int damage;
    private float knockback;

    private List<Collider> alreadyColliderWith = new List<Collider>();

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
            Debug.Log("플레이어한테 공격 성공");
        }

        if (other.TryGetComponent(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
        }
    }

    public void SetAttack(int damage, float knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
