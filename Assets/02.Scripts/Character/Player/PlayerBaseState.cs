using UnityEngine;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
        Move();
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }


    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);

        Move(movementDirection);
    }

    protected Vector3 GetMovementDirection()
    {

        //Vector3 dir = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).normalized;
        //return dir;

        if (stateMachine.GetCurrentTarget() != null)
        {
            Vector3 dir = (stateMachine.GetCurrentTarget().transform.position - stateMachine.Player.transform.position).normalized;
            return dir;
        }
        else
        {
            return Vector3.zero;
        }

    }

    void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        //Debug.Log("movementDirection" + movementDirection);
        //Debug.Log("movementSpeed" + movementSpeed);

        stateMachine.Player.Controller.Move(((movementDirection * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);
    }


    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier * stateMachine.Player.moveSpeed; // 플레이어의 스피드 곱해주기(레벨업이나 아이템으로 인해 증가 되었을 때)
        return movementSpeed;
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    protected void ForceMove()
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
    }


    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime;
        }
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }


    /*
    protected bool IsInChasingRange()
    {
        float enemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;

        return enemyDistanceSqr <= stateMachine.Player.Data.EnemyChasingRange * stateMachine.Player.Data.EnemyChasingRange;
    }

    protected bool IsInAttackRange()
    {
        float enemyDistanceSqr = (stateMachine.Target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;

        return enemyDistanceSqr <= stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;
    }
    */




    protected bool IsInChasingRange()
    {
        Debug.Log("target count " + stateMachine.Targets.Count);
        foreach (HealthSystem target in stateMachine.Targets)
        {
            if (target != null)
            {
                float enemyDistanceSqr = (target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
                float chasingRangeSqr = stateMachine.Player.Data.EnemyChasingRange * stateMachine.Player.Data.EnemyChasingRange;

                Debug.Log("baseChasing" + target.name);
                if (enemyDistanceSqr <= chasingRangeSqr)
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected bool IsInAttackRange()
    {
        foreach (HealthSystem target in stateMachine.Targets)
        {
            if (target != null)
            {
                float enemyDistanceSqr = (target.transform.position - stateMachine.Player.transform.position).sqrMagnitude;
                float attackRangeSqr = stateMachine.Player.Data.AttackRange * stateMachine.Player.Data.AttackRange;

                if (enemyDistanceSqr <= attackRangeSqr)
                {
                    Debug.Log("baseAttack");
                    return true;
                }
            }
        }

        return false;
    }



}
