using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private bool alreadyAppliedForce;
    private bool alreadyShooting;

    private HealthSystem currentTarget;

    private AttackInfoData attackInfoData;

    private bool hasWeapon; // 무기를 가지고 있는지 여부

    private float shootInterval = 0.2f; // 발사 간격 설정

    private float shootTimer = 0f; // 발사 간격을 계산할 타이머 변수

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
        // 무기 관련 상태 초기화
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
                // 발사 간격 체크
                if (CanShoot())
                {
                    Shooting();
                    Debug.Log("슈팅호출");
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

    public void Shooting() // 무기(가발) 발사
    {

        // 현재 타겟 가져오기
        //HealthSystem target = stateMachine.GetCurrentTarget();

        // 무기를 가지고 있는지 확인
        hasWeapon = stateMachine.Player.currentWeapon != null;

        if (!hasWeapon)
        {
            // 무기가 없으면 생성
            stateMachine.Player.SpawnWeapon();
        }

        GameObject weapon = stateMachine.Player.currentWeapon;

        if (weapon != null)
        {
            // 타겟 에너미 위치 가져오기
            //Vector3 targetPosition = stateMachine.Target.transform.position;
            //Vector3 targetPosition = stateMachine.GetCurrentTarget().transform.position;
            Vector3 targetPosition = currentTarget.transform.position;

            // 무기를 플레이어에서 분리
            weapon.transform.SetParent(null);

            // 무기를 타겟 에너미 방향으로 발사
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
