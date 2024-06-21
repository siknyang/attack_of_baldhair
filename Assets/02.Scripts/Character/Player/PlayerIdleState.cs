using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : BaseState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (GetMovementDirection() != Vector3.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }
}
