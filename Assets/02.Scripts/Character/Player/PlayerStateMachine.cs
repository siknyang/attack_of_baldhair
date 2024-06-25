using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public List<HealthSystem> Targets { get; private set; } // 여러 개의 타겟을 관리하기 위해 List 사용
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerAttackState AttackState { get; }

    // 움직임 관련 데이터
    public Vector2 MoveDirection { get; set; } // 움직임 방향
    public float MovementSpeed { get; private set; } // 이동 속도
    public float RotationDamping { get; private set; } // 회전 감속도
    public float MovementSpeedModifier { get; set; } = 1f; // 이동 속도 보정

    private float detectionInterval = 1f; // 주기적 감지 간격
    private float detectionTimer = 0f;


    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        //Target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<HealthSystem>();

        Targets = new List<HealthSystem>(); // 여러 개의 타겟을 관리할 리스트 생성

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);

        AttackInfoData attackInfoData = player.Data.AttackInfoData;
        AttackState = new PlayerAttackState(this, attackInfoData);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }

    public void Update()
    {
        base.Update(); // 이게 없어서 StateMachine 여길 통해서 못가서??

        detectionTimer += Time.deltaTime;

        if (detectionTimer >= detectionInterval)
        {
            detectionTimer = 0f;
            DetectAndAddTargets();
        }

        // 가장 가까운 타겟을 가져옴
        HealthSystem closestTarget = GetClosestTarget();

        if (closestTarget != null)
        {
            // 공격 상태로 전환
            //ChangeState(AttackState);

            // AttackState에게 현재 타겟 설정
            AttackState.SetTarget(closestTarget);
        }
    }

    private void DetectAndAddTargets()
    {
        // 플레이어 주변의 적을 탐지하여 Targets 리스트에 추가
        Collider[] colliders = Physics.OverlapSphere(Player.transform.position, 10f, LayerMask.GetMask("Enemy")); // Enemy 레이어의 콜라이더들을 가져옴

        foreach (var collider in colliders)
        {
            HealthSystem healthSystem = collider.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                AddTarget(healthSystem); // Targets 리스트에 적 추가
            }
        }
    }

    public void ClearTargets()
    {
        Targets.Clear();
    }

    public void AddTarget(HealthSystem obj)
    {
        if (!Targets.Contains(obj)) // 이미 있는 타겟이 아닌 경우에만 추가
        {
            Targets.Add(obj);
            Debug.Log("타켓 추가");
        }
    }

    public void RemoveTarget(HealthSystem obj)
    {
        Targets.Remove(obj);// 해당 타겟을 리스트에서 제거
        Debug.Log("리스트에서 타겟 제거");
    }

    // 가장 가까운 타겟을 반환하는 메서드
    public HealthSystem GetClosestTarget()
    {
        HealthSystem closestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 playerPosition = Player.transform.position;

        foreach (HealthSystem target in Targets)
        {
            if (target != null)
            {
                float distance = Vector3.Distance(playerPosition, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        return closestTarget;
    }

    // 현재 타겟을 반환하는 메서드
    public HealthSystem GetCurrentTarget()
    {
        return Targets.Count > 0 ? Targets[0] : null; // 여기서는 첫 번째 타겟을 반환하도록 설정
    }

}
