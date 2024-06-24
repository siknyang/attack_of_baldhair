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



    public GameObject WeaponPrefab; // ���� ������
    public Transform SpawnPos; // ���� ���� ��ġ
    private GameObject currentWeapon; // ���� �տ� ��� �ִ� ����

    [field: SerializeField] public PlayerWeapon Weapon { get; private set; }



    private PlayerStateMachine stateMachine;

    public HealthSystem Health {  get; private set; }

    private void Awake()
    {

        stateMachine = new PlayerStateMachine(this);

        AnimationData.Initialize();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Health = GetComponent<HealthSystem>();

        SpawnWeapon(); // ���� ����
    }

    private void Start()
    {
        LoadData();     // ������ ������ �� ����� ������ �ҷ�����
        stateMachine.ChangeState(stateMachine.IdleState);
        Health.OnDie += OnDie;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void SpawnWeapon()
    {
        if (WeaponPrefab != null)
        {
            currentWeapon = Instantiate(WeaponPrefab, SpawnPos.position, SpawnPos.localRotation);
            currentWeapon.transform.SetParent(SpawnPos);
        }
    }

    public GameObject GetCurrentWeapon()
    {
        return currentWeapon;
    }

    private void OnDie()
    {
        Animator.SetTrigger("Die");
        enabled = false;
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
            //health = 100;
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

        DataManager.Instance.SaveData(data);
    }

    private void OnApplicationQuit()    // ������ ���� �� ����
    {
        SaveData();
    }
}
