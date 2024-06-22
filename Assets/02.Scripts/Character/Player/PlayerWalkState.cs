using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        base.Update();
        // �÷��̾� �ֺ��� ���� �����Ǹ� ���� ���·� ��ȯ
        if (GetMovementDirection() != Vector3.zero)
        {
            Transform nearestEnemy = stateMachine.GetClosestEnemyInAttackRange();
            if (nearestEnemy != null)
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                return; // ���� ��ȯ �� ��� ����
            }
        }
    }
}
