using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterStats
{
    public CharacterController Controller { get; private set; }

    private PlayerStateMachine stateMachine;

    private void Awake()
    {
        level = 1;
        health = 100;
        attackPower = 10;
        attackSpeed = 1.0f;
        moveSpeed = 1.0f;
        attackRange = 3.0f;
        experience = 0;
        coin = 0;

        stateMachine = new PlayerStateMachine(this);
        Debug.Log("�÷��̾�" + stateMachine.MovementSpeed);

        Controller = GetComponent<CharacterController>();
        if (Controller == null)
        {
            Debug.Log("ĳ������Ʈ�ѷ� ����");
        }
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        Debug.Log("�÷��̾�" + stateMachine.MovementSpeed);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

}