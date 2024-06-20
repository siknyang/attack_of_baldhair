using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerWalkState : BaseState
{

    private const float WalkSpeedModifier = 1.0f;

    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = WalkSpeedModifier;
        base.Enter();
        Debug.Log("walkState Enter");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("walkState Exit");
    }

    public override void Update()
    {
        base.Update();
    }
}
