using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private bool alreadyAppliedForce;
    private bool alreadyThrownWeapon;

    private AttackInfoData attackInfoData;

    private bool hasWeapon; // 무기를 가지고 있는지 여부

    public PlayerAttackState(PlayerStateMachine stateMachine, AttackInfoData attackInfoData) : base(stateMachine)
    {
        this.attackInfoData = attackInfoData;
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StartAnimation(stateMachine.Player.AnimationData.JustAttackParameterHash);

        alreadyAppliedForce = false;
        alreadyThrownWeapon = false;

        // 무기를 가지고 있는지 확인
        hasWeapon = stateMachine.Player.GetCurrentWeapon() != null;

        if (!hasWeapon)
        {
            // 무기가 없으면 생성
            stateMachine.Player.SpawnWeapon();
        }
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Player.AnimationData.JustAttackParameterHash);

    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "JustAttack");
        if (normalizedTime < 1f)
        {
            if (normalizedTime >= attackInfoData.ForceTransitionTime)
                TryApplyForce();

            if (!alreadyThrownWeapon && normalizedTime >= stateMachine.Player.Data.AttackInfoData.Dealing_Start_TransitionTime)
            {
                ThrowWeapon();
                alreadyThrownWeapon = true;
            }
        }
        else
        {
            if (IsInChasingRange())
            {
                stateMachine.ChangeState(stateMachine.WalkState);
                return;
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        alreadyAppliedForce = true;

        stateMachine.Player.ForceReceiver.Reset();

        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }

    private void ThrowWeapon() // 무기 투척
    {
        GameObject weapon = stateMachine.Player.GetCurrentWeapon();

        if (weapon != null)
        {
            // 타겟 에너미 위치 가져오기
            Vector3 targetPosition = stateMachine.Target.transform.position;

            // 무기를 타겟 에너미 방향으로 던지기
            Rigidbody weaponRigidbody = weapon.GetComponent<Rigidbody>();
            Vector3 throwDirection = (targetPosition - weapon.transform.position).normalized;
            float throwForce = 5f; // 던지는 힘
            weaponRigidbody.velocity = throwDirection * throwForce;
            //weaponRigidbody.AddForce(throwDirection *  throwForce, ForceMode.Impulse);

            // 무기를 플레이어의 손에서 분리??
            weapon.transform.SetParent(null);
        }
    }
}
