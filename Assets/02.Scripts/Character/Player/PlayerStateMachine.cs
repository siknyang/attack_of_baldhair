using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public HealthSystem Target { get; private set; }
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerAttackState AttackState { get; }

    // ������ ���� ������
    public Vector2 MoveDirection { get; set; } // ������ ����
    public float MovementSpeed { get; private set; } // �̵� �ӵ�
    public float RotationDamping { get; private set; } // ȸ�� ���ӵ�
    public float MovementSpeedModifier { get; set; } = 1f; // �̵� �ӵ� ����

    public PlayerStateMachine(Player player)
    {
        this.Player = player;
        Target = GameObject.FindGameObjectWithTag("Enemy").GetComponent<HealthSystem>();

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);

        AttackInfoData attackInfoData = player.Data.AttackInfoData;
        AttackState = new PlayerAttackState(this, attackInfoData);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        RotationDamping = player.Data.GroundData.BaseRotationDamping;
    }

}
