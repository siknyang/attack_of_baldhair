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

        Controller = GetComponent<CharacterController>();
        if (Controller == null)
        {
            Debug.Log("캐릭터컨트롤러 없음");
        }
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

}
