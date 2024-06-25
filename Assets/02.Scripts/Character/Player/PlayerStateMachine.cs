using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public List<HealthSystem> Targets { get; private set; } // ���� ���� Ÿ���� �����ϱ� ���� List ���
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerAttackState AttackState { get; }

    // ������ ���� ������
    public Vector2 MoveDirection { get; set; } // ������ ����
    public float MovementSpeed { get; private set; } // �̵� �ӵ�
    public float RotationDamping { get; private set; } // ȸ�� ���ӵ�
    public float MovementSpeedModifier { get; set; } = 1f; // �̵� �ӵ� ����

    private float detectionInterval = 1f; // �ֱ��� ���� ����
    private float detectionTimer = 0f;


    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        //Target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<HealthSystem>();

        Targets = new List<HealthSystem>(); // ���� ���� Ÿ���� ������ ����Ʈ ����

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);

        AttackInfoData attackInfoData = player.Data.AttackInfoData;
        AttackState = new PlayerAttackState(this, attackInfoData);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }

    public void Update()
    {
        base.Update(); // �̰� ��� StateMachine ���� ���ؼ� ������??

        detectionTimer += Time.deltaTime;

        if (detectionTimer >= detectionInterval)
        {
            detectionTimer = 0f;
            DetectAndAddTargets();
        }

        // ���� ����� Ÿ���� ������
        HealthSystem closestTarget = GetClosestTarget();

        if (closestTarget != null)
        {
            // ���� ���·� ��ȯ
            //ChangeState(AttackState);

            // AttackState���� ���� Ÿ�� ����
            AttackState.SetTarget(closestTarget);
        }
    }

    private void DetectAndAddTargets()
    {
        // �÷��̾� �ֺ��� ���� Ž���Ͽ� Targets ����Ʈ�� �߰�
        Collider[] colliders = Physics.OverlapSphere(Player.transform.position, 10f, LayerMask.GetMask("Enemy")); // Enemy ���̾��� �ݶ��̴����� ������

        foreach (var collider in colliders)
        {
            HealthSystem healthSystem = collider.GetComponent<HealthSystem>();
            if (healthSystem != null)
            {
                AddTarget(healthSystem); // Targets ����Ʈ�� �� �߰�
            }
        }
    }

    public void ClearTargets()
    {
        Targets.Clear();
    }

    public void AddTarget(HealthSystem obj)
    {
        if (!Targets.Contains(obj)) // �̹� �ִ� Ÿ���� �ƴ� ��쿡�� �߰�
        {
            Targets.Add(obj);
            Debug.Log("Ÿ�� �߰�");
        }
    }

    public void RemoveTarget(HealthSystem obj)
    {
        Targets.Remove(obj);// �ش� Ÿ���� ����Ʈ���� ����
        Debug.Log("����Ʈ���� Ÿ�� ����");
    }

    // ���� ����� Ÿ���� ��ȯ�ϴ� �޼���
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

    // ���� Ÿ���� ��ȯ�ϴ� �޼���
    public HealthSystem GetCurrentTarget()
    {
        return Targets.Count > 0 ? Targets[0] : null; // ���⼭�� ù ��° Ÿ���� ��ȯ�ϵ��� ����
    }

}
