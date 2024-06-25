using System.Collections;
using UnityEngine;

public class Enemy : CharacterStats
{
    [field: Header("Reference")]
    [field: SerializeField] public EnemySO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData AnimationData { get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private EnemyStateMachine stateMachine;

    public HealthSystem Health { get; private set; }

    [field:SerializeField] public EnemyWeapon Weapon { get; private set; }

    private Coroutine deathCoroutine;

    [SerializeField] private GameObject hairActivate; // 활성화할 머리
    private Renderer[] renderers;

    private void Awake()
    {
        health = 50;
        attackPower = 8;
        attackSpeed = 1.2f;
        experience = 50;

        AnimationData.Initialize();

        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Health = GetComponent<HealthSystem>();

        renderers = GetComponentsInChildren<Renderer>();

        stateMachine = new EnemyStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        Health.OnDie += OnDie;
        Health.OnTakeDamage += OnTakeDamage;
        Health.IsEnemy = true;
        Weapon.damage = Data.Damage;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void OnDie()
    {
        if (hairActivate != null)
        {
            hairActivate.SetActive(true);
        }

        Controller.enabled = false; // 다시 사용할 땐 true로 바꿔줘야 함
        Animator.SetTrigger("Die");
        enabled = false;

        // 플레이어에게 경험치 부여
        FindObjectOfType<Player>().AddExperience(experience);

        // 타겟에서 삭제
        FindObjectOfType<Player>().stateMachine.RemoveTarget(Health);

        // 3초 후에 자동으로 오브젝트 삭제 코루틴 시작
        deathCoroutine = StartCoroutine(DestroyAfterDelay(3f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // 오브젝트를 파괴
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // 코루틴이 실행 중인 경우 취소
        if (deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine);
        }
    }

    private void OnTakeDamage()
    {
        StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()// 공격당할 때마다 블링크
    {
        Color originalColor = renderers[0].material.color;
        Color blinkColor = new Color(1f, 0.5f, 0.5f, 0.4f);

        foreach (var renderer in renderers)
        {
            renderer.material.color = blinkColor;
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var renderer in renderers)
        {
            renderer.material.color = originalColor;
        }
    }

    private void OnDrawGizmosSelected()// 에너미의 타켓(플레이어)공격/추적(감지) 범위 기즈모
    {
        if (Data == null) return;

        // 공격 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);

        // 추적 범위
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Data.PlayerChasingRange);
    }
}