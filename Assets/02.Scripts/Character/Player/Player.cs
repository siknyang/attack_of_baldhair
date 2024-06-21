using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterStats
{
    public CharacterController Controller { get; private set; }

    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        LoadData();     // 게임이 시작할 때 저장된 데이터 불러오기

        //level = 1;
        //health = 100;
        //attackPower = 10;
        //attackSpeed = 1.0f;
        //moveSpeed = 1.0f;
        //attackRange = 3.0f;
        //experience = 0;
        //coin = 0;

        stateMachine = new PlayerStateMachine(this);
        Debug.Log("플레이어" + stateMachine.MovementSpeed);

        Controller = GetComponent<CharacterController>();
        if (Controller == null)
        {
            Debug.Log("캐릭터컨트롤러 없음");
        }
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        Debug.Log("플레이어" + stateMachine.MovementSpeed);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    private void LoadData()     // 불러온 데이터
    {
        UserData data = DataManager.Instance.LoadData<UserData>();
        
        if (data == null)   // 데이터가 null이라면
        {
            // 초기값 세팅
            level = 1;
            health = 100;
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

        DataManager.instance.SaveData(data);
    }

    private void OnApplicationQuit()    // 게임이 끝날 때 저장
    {
        SaveData();
    }
}
