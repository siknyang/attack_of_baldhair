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
        Debug.Log("idleState Enter");
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("idleState Exit");
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
