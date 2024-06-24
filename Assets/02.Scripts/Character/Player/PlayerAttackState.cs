using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private bool alreadyAppliedForce;
    private bool alreadyShooting;

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
        alreadyShooting = false;

    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Player.AnimationData.JustAttackParameterHash);
       // alreadyShooting = false;
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

            if (!alreadyShooting && normalizedTime >= stateMachine.Player.Data.AttackInfoData.Dealing_Start_TransitionTime)
            {
                Shooting();
                //alreadyShooting = true;
            }
        }
        else
        {
            if (IsInAttackRange())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                return;
            }

            else if (IsInChasingRange())
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


    
    public void Shooting() // ����(����) �߻�
    {
        Debug.Log("���� ����");

        // ���⸦ ������ �ִ��� Ȯ��
        hasWeapon = stateMachine.Player.currentWeapon != null;

        if (!hasWeapon)
        {
            // ���Ⱑ ������ ����
            stateMachine.Player.SpawnWeapon();
        }

        GameObject weapon = stateMachine.Player.currentWeapon;

        if (weapon != null)
        {
            // Ÿ�� ���ʹ� ��ġ ��������
            Vector3 targetPosition = stateMachine.Target.transform.position;

            // ���⸦ �÷��̾�� �и�
            weapon.transform.SetParent(null);

            // ���⸦ Ÿ�� ���ʹ� �������� �߻�
            Rigidbody weaponRigidbody = weapon.GetComponent<Rigidbody>();
            Vector3 shootDirection = (targetPosition - weapon.transform.position).normalized;
            float shootForce = 10f;
            weaponRigidbody.velocity = shootDirection * shootForce;
            //weaponRigidbody.AddForce(throwDirection *  throwForce, ForceMode.Impulse);

            stateMachine.Player.currentWeapon = null;
        }
    }
    


}
