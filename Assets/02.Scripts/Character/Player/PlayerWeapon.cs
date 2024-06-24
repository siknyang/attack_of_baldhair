using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private int damage;
    private float knockback;

    private float destroyTime = 3.0f; // 무기를 제거할 시간
    private bool hitEnemy = false; // 적을 맞췄는지 여부

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
            hitEnemy = true; // 적을 맞췄음을 표시
        }

        if (other.TryGetComponent(out ForceReceiver forceReceiver))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            forceReceiver.AddForce(direction * knockback);
            hitEnemy = true; // 적을 맞췄음을 표시
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

        if (!hitEnemy) // 적을 맞추지 않았을 경우
        {
            Destroy(gameObject); // 삭제
        }
    }
}
