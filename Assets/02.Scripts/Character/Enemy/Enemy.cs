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

    [SerializeField] private GameObject hairActivate; // Ȱ��ȭ�� �Ӹ�
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

        Controller.enabled = false; // �ٽ� ����� �� true�� �ٲ���� ��
        Animator.SetTrigger("Die");
        enabled = false;

        // �÷��̾�� ����ġ �ο�
        FindObjectOfType<Player>().AddExperience(experience);

        // Ÿ�ٿ��� ����
        FindObjectOfType<Player>().stateMachine.RemoveTarget(Health);

        // 3�� �Ŀ� �ڵ����� ������Ʈ ���� �ڷ�ƾ ����
        deathCoroutine = StartCoroutine(DestroyAfterDelay(3f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ������Ʈ�� �ı�
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // �ڷ�ƾ�� ���� ���� ��� ���
        if (deathCoroutine != null)
        {
            StopCoroutine(deathCoroutine);
        }
    }

    private void OnTakeDamage()
    {
        StartCoroutine(BlinkEffect());
    }

    private IEnumerator BlinkEffect()// ���ݴ��� ������ ��ũ
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

    private void OnDrawGizmosSelected()// ���ʹ��� Ÿ��(�÷��̾�)����/����(����) ���� �����
    {
        if (Data == null) return;

        // ���� ����
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange);

        // ���� ����
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Data.PlayerChasingRange);
    }
}