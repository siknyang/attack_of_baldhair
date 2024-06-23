using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private bool alreadyAppliedForce;
    private bool alreadyThrownWeapon;

    private AttackInfoData attackInfoData;

    private bool hasWeapon; // ���⸦ ������ �ִ��� ����

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

        // ���⸦ ������ �ִ��� Ȯ��
        hasWeapon = stateMachine.Player.GetCurrentWeapon() != null;

        if (!hasWeapon)
        {
            // ���Ⱑ ������ ����
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

    private void ThrowWeapon() // ���� ��ô
    {
        GameObject weapon = stateMachine.Player.GetCurrentWeapon();

        if (weapon != null)
        {
            // Ÿ�� ���ʹ� ��ġ ��������
            Vector3 targetPosition = stateMachine.Target.transform.position;

            // ���⸦ Ÿ�� ���ʹ� �������� ������
            Rigidbody weaponRigidbody = weapon.GetComponent<Rigidbody>();
            Vector3 throwDirection = (targetPosition - weapon.transform.position).normalized;
            float throwForce = 5f; // ������ ��
            weaponRigidbody.velocity = throwDirection * throwForce;
            //weaponRigidbody.AddForce(throwDirection *  throwForce, ForceMode.Impulse);

            // ���⸦ �÷��̾��� �տ��� �и�??
            weapon.transform.SetParent(null);
        }
    }
}
