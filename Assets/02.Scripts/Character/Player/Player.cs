using System.Collections;
using System.Collections.Generic;
using System;
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

    public GameObject currentWeapon; // 현재 손에 들고 있는 무기

    [field: SerializeField] public PlayerWeapon Weapon { get; private set; }

    public PlayerStateMachine stateMachine;

    public HealthSystem Health {  get; private set; }

    public float experienceToNextLevel = 100f; // 레벨업에 필요한 기본 경험치

    public event Action OnExperienceChanged;
    public event Action OnLevelChanged;

    // 아이템 관련
    //private ItemData equippedItem; // 장착된 아이템
    private List<ItemSO> equippdeItems = new List<ItemSO>();

    private void Awake()
    {

        stateMachine = new PlayerStateMachine(this);

        AnimationData.Initialize();
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        Health = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        LoadData();     // 게임이 시작할 때 저장된 데이터 불러오기
        stateMachine.ChangeState(stateMachine.IdleState);
        Health.OnDie += OnDie;
        OnExperienceChanged?.Invoke(); // 초기 UI 업데이트
        OnLevelChanged?.Invoke(); // 초기 UI 업데이트
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
            Debug.Log("무기 생성");
            currentWeapon = Instantiate(WeaponPrefab, SpawnPos.position, Quaternion.identity);
            currentWeapon.GetComponent<PlayerWeapon>().damage = Data.AttackInfoData.Damage;
            currentWeapon.transform.SetParent(SpawnPos);
        }
    }

        /*
    // 아이템 장착
    public void EquipItem(ItemData item)
    {
        if (equippedItem != null)
        {
            // 이미 장착된 아이템이 있다면 스탯 감소
            DecreaseStats(equippedItem.itemName);
        }

        equippedItem = item;

        // 새로운 아이템 장착 시 스탯 증가
        IncreaseStats(item.itemName);
        Debug.Log("아이템 장착: " + item.itemName);
    }

    // 아이템 해지
    public void UnequipItem()
    {
        if (equippedItem != null)
        {
            DecreaseStats(equippedItem.itemName);
            Debug.Log("아이템 해지: " + equippedItem.itemName);
            equippedItem = null;
        }
    }
    */

    public void EquipInventoryItem(ItemSO item) // 위 유정님 작성 스크립트 참고
    {
        equippdeItems.Add(item);

        switch (item.itemName)
        {
            case "트리트먼트":
                attackSpeed += 5;
                Debug.Log($"트리트먼트 장착 : 공격속도 {attackSpeed}");
                break;
            case "손":
                attackPower += 10;
                Debug.Log($"손 장착 : 공격속도 {attackPower}");
                break;
        }
        Debug.Log($"아이템 장착 : {item.itemName}");
    }

    public void AddExperience(float xp)
    {
        experience += xp;
        Debug.Log("플레이어 경험치 획득 : " + experience);

        while (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
        OnExperienceChanged?.Invoke();
    }

    private void LevelUp()
    {
        level++;
        experience -= experienceToNextLevel;
        experienceToNextLevel = Mathf.RoundToInt(experienceToNextLevel * 1.25f); // 다음 레벨업에 필요한 경험치 증가
        attackPower += 2; // 레벨업 시 공격력 증가
        attackSpeed += 0.1f; // 레벨업 시 공격 속도 증가

        Debug.Log("현재 플레이어 레벨: " + level);
        OnLevelChanged?.Invoke();
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
