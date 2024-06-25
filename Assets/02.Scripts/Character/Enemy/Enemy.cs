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

        stateMachine = new EnemyStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        Health.OnDie += OnDie;

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
        Animator.SetTrigger("Die");
        enabled = false;
        //FindObjectOfType<Player>().stateMachine.ClearTarget();
        // �װ� ���ο� ���ʹ� �����ؼ� �¸� ���ο� �÷��̾��� Ÿ������ �����

        // �÷��̾�� ����ġ �ο�
        FindObjectOfType<Player>().AddExperience(experience);
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