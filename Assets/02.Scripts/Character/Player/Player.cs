using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterStats
{
    [field:SerializeField] public PlayerSO Data { get; private set; }

    [field:Header("Animations")]
    [field:SerializeField] public PlayerAnimationData AnimationData {  get; private set; }

    public Animator Animator { get; private set; }

    public CharacterController Controller { get; private set; }

    public ForceReceiver ForceReceiver { get; private set; }

    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        LoadData();     // ������ ������ �� ����� ������ �ҷ�����

        stateMachine = new PlayerStateMachine(this);

        Controller = GetComponent<CharacterController>();
        if (Controller == null)
        {
            Debug.Log("ĳ������Ʈ�ѷ� ����");
        }

        ForceReceiver = GetComponent<ForceReceiver>();
    }

    private void Start()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
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
