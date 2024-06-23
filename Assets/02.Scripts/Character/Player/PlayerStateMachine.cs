using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public HealthSystem Target { get; private set; }
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerAttackState AttackState { get; }

    // 움직임 관련 데이터
    public Vector2 MoveDirection { get; set; } // 움직임 방향
    public float MovementSpeed { get; private set; } // 이동 속도
    public float RotationDamping { get; private set; } // 회전 감속도
    public float MovementSpeedModifier { get; set; } = 1f; // 이동 속도 보정

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
