using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerAttackState AttackState { get; }
    public PlayerJustAttackState JustAttackState { get; }
    //public PlayerHitState HitState { get; }
    //public PlayerDieState DieState { get; }

    // ������ ���� ������
    public Vector3 MoveDirection { get; set; } // ������ ����

    public float MovementSpeed { get; private set; } // �̵� �ӵ�
    public float RotationDamping { get; private set; } // ȸ�� ���ӵ�
    public float MovementSpeedModifier { get; set; } = 1f; // �̵� �ӵ� ����

    public bool IsAttacking { get; set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        AttackState = new PlayerAttackState(this);
        JustAttackState = new PlayerJustAttackState(this);
        //HitState = new PlayerHitState(this);
        //DieState = new PlayerDieState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
        MovementSpeedModifier = 1f;
    }

    public void Update()
    {
        // �ֺ��� ���� �ִ��� Ȯ��
        Transform nearestEnemy = GetClosestEnemyInAttackRange();

        // ���� �����Ǹ� ���� ���·� ��ȯ
        if (nearestEnemy != null)
        {
            ChangeState(AttackState);
        }
        else
        {
            // ���� ������ �⺻ ���·� ��ȯ (Idle ����)
            ChangeState(IdleState);
        }

        // ���� ���� ������Ʈ
        currentState?.Update();
    }


    // ���� ����� ���� ã�� �޼���
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
