using UnityEngine;

public class Player : CharacterStats
{
    [field:SerializeField] public PlayerSO Data { get; private set; }

    [field:Header("Animations")]
    [field:SerializeField] public PlayerAnimationData AnimationData {  get; private set; }

    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }

    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        LoadData();     // ������ ������ �� ����� ������ �ҷ�����

        stateMachine = new PlayerStateMachine(this);

        AnimationData.Initialize();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void OnDrawGizmosSelected() // �÷��̾��� Ÿ��(���ʹ�)����/����(����) ���� �����
    {
        if (Data == null) return;

        // ���� ����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange); // �÷��̾�� ���Ÿ� Ÿ���̶� ���ʹ̺��� ���� ������ ����

        // ���� ����
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Data.EnemyChasingRange);
    }

    private void LoadData()     // �ҷ��� ������
    {
        UserData data = DataManager.Instance.LoadData<UserData>();
        
        if (data == null)   // �����Ͱ� null�̶��
        {
            // �ʱⰪ ����
            level = 1;
            health = 100;
            attackPower = 10;
            attackSpeed = 1.0f;
            moveSpeed = 1.0f;
            attackRange = 3.0f;
            experience = 0;
            coin = 0;
        }
        else    // null�� �ƴ϶�� ����� ������ �ҷ��ͼ� ���� ����
        {
            level = data.level;
            attackPower = data.attackPower;
            attackSpeed = data.attackSpeed;
            experience = data.experience;
            coin = data.coin;
        }
    }

    private void SaveData()     // ������ ������
    {
        UserData data = new UserData();

        data.level = level;
        data.attackPower = attackPower; 
        data.attackSpeed = attackSpeed;
        data.experience = experience;
        data.coin = coin;

        DataManager.instance.SaveData(data);
    }

    private void OnApplicationQuit()    // ������ ���� �� ����
    {
        SaveData();
    }
}
