using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private bool alreadyAppliedForce;
    private bool alreadyShooting;

    private HealthSystem currentTarget;

    private AttackInfoData attackInfoData;

    private bool hasWeapon; // ���⸦ ������ �ִ��� ����

    private float shootInterval = 0.2f; // �߻� ���� ����

    private float shootTimer = 0f; // �߻� ������ ����� Ÿ�̸� ����

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
        // ���� ���� ���� �ʱ�ȭ
        alreadyShooting = false;
        hasWeapon = false;
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "JustAttack");
        //if (normalizedTime < 1f)
        //{
            if (normalizedTime >= attackInfoData.ForceTransitionTime)
                TryApplyForce();

            //if (!alreadyShooting && normalizedTime >= stateMachine.Player.Data.AttackInfoData.Dealing_Start_TransitionTime)
            //{
                // �߻� ���� üũ
                if (CanShoot())
                {
                    Shooting();
                    Debug.Log("����ȣ��");
                }
            //}
        //}
       // else
        //{
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
        //}
    }

    private bool CanShoot()
    {
        if (Time.time - shootTimer > shootInterval)
        {
            shootTimer = Time.time;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedForce) return;
        alreadyAppliedForce = true;

        stateMachine.Player.ForceReceiver.Reset();

        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }

    public void SetTarget(HealthSystem target)
    {
        currentTarget = target;
    }

    public void Shooting() // ����(����) �߻�
    {

        // ���� Ÿ�� ��������
        //HealthSystem target = stateMachine.GetCurrentTarget();

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
            //Vector3 targetPosition = stateMachine.Target.transform.position;
            //Vector3 targetPosition = stateMachine.GetCurrentTarget().transform.position;
            Vector3 targetPosition = currentTarget.transform.position;

            // ���⸦ �÷��̾�� �и�
            weapon.transform.SetParent(null);

            // ���⸦ Ÿ�� ���ʹ� �������� �߻�
            Rigidbody weaponRigidbody = weapon.GetComponent<Rigidbody>();
            Vector3 shootDirection = (targetPosition - weapon.transform.position).normalized;
            float shootForce = 10f;
            weaponRigidbody.velocity = shootDirection * shootForce;
            //weaponRigidbody.AddForce(throwDirection *  throwForce, ForceMode.Impulse);

            SoundManager.instance.PlayPlayerAttackSFX();

            stateMachine.Player.currentWeapon = null;
        }
    }



}
