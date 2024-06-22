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

        if (stateMachine.IsAttacking)
        {
            OnAttack();
            return;
        }
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
        /*
        Vector3 forward = stateMachine.MainCamTransform.forward;
        Vector3 right = stateMachine.MainCamTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
        */

        // ���� ����� ���� ��ġ ������
        Transform nearestEnemy = stateMachine.GetClosestEnemyInAttackRange();

        if (nearestEnemy != null)
        {
            // ���� ��ġ�� Ÿ������ �����ϰ�, �� ������ ����
            Vector3 targetPosition = nearestEnemy.position;
            Vector3 moveDirection = (targetPosition - stateMachine.Player.transform.position).normalized;

            // ���� ��ȯ
            return moveDirection;
        }
        else
        {
            // ���� ������ �������� �ʵ��� Vector3.zero�� ��ȯ
            return Vector3.zero;
        }
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();
        //stateMachine.Player.Controller.Move(((direction * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime);
        stateMachine.Player.transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        Debug.Log("Move speed: " + movementSpeed);
    }


    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier * stateMachine.Player.moveSpeed; // �÷��̾��� ���ǵ� �����ֱ�(�������̳� ���������� ���� ���� �Ǿ��� ��)
        Debug.Log("Movement Speed: " + stateMachine.MovementSpeed);
        Debug.Log("movementSpeedModifier: " + stateMachine.MovementSpeedModifier);
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

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.AttackState);
    }
}
