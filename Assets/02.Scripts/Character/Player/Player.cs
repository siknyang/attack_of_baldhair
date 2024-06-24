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



    public GameObject WeaponPrefab; // 무기 프리팹
    public Transform SpawnPos; // 무기 생성 위치
    private GameObject currentWeapon; // 현재 손에 들고 있는 무기

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

        SpawnWeapon(); // 무기 생성
    }

    private void Start()
    {
        LoadData();     // 게임이 시작할 때 저장된 데이터 불러오기
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

    private void OnDrawGizmosSelected() // 플레이어의 타켓(에너미)공격/추적(감지) 범위 기즈모
    {
        if (Data == null) return;

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Data.AttackRange); // 플레이어는 원거리 타입이라 에너미보다 공격 범위가 넓음

        // 추적 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, Data.EnemyChasingRange);
    }

    private void LoadData()     // 불러온 데이터
    {
        UserData data = DataManager.Instance.LoadData<UserData>();
        
        if (data == null)   // 데이터가 null이라면
        {
            // 초기값 세팅
            level = 1;
            //health = 100;
            attackPower = 10;
            attackSpeed = 1.0f;
            moveSpeed = 1.0f;
            attackRange = 3.0f;
            experience = 0;
            coin = 0;
        }
        else    // null이 아니라면 저장된 데이터 불러와서 덮어 씌움
        {
            level = data.level;
            attackPower = data.attackPower;
            attackSpeed = data.attackSpeed;
            experience = data.experience;
            coin = data.coin;
        }
    }

    private void SaveData()     // 저장할 데이터
    {
        UserData data = new UserData();

        data.level = level;
        data.attackPower = attackPower; 
        data.attackSpeed = attackSpeed;
        data.experience = experience;
        data.coin = coin;

        DataManager.Instance.SaveData(data);
    }

    private void OnApplicationQuit()    // 게임이 끝날 때 저장
    {
        SaveData();
    }
}
