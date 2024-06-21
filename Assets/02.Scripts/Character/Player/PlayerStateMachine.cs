using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerAttackState AttackState { get; }
    //public PlayerHitState HitState { get; }
    //public PlayerDieState DieState { get; }

    // 움직임 관련 데이터
    public Vector3 MoveDirection { get; set; } // 움직임 방향

    public float MovementSpeed { get; private set; } // 이동 속도
    public float RotationDamping { get; private set; } // 회전 감속도
    public float MovementSpeedModifier { get; set; } = 1f; // 이동 속도 보정

    public bool IsAttacking { get; set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        AttackState = new PlayerAttackState(this);
        //HitState = new PlayerHitState(this);
        //DieState = new PlayerDieState(this);

        MovementSpeed = player.Data.PlayerData.BaseSpeed;
        RotationDamping = player.Data.PlayerData.BaseRotationDamping;
        //MovementSpeedModifier = 1f;
    }

    // 가장 가까운 적을 찾는 메서드
    public Transform GetClosestEnemyInAttackRange()
    {
        Collider[] colliders = Physics.OverlapSphere(Player.transform.position, Player.attackRange, LayerMask.GetMask("Enemy"));

        Transform closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            float distance = Vector3.Distance(Player.transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = collider.transform;
            }
        }

        return closestEnemy;
    }
}
