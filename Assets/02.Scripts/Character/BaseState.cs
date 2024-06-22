using UnityEngine;

public class BaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerData playerData;

    public BaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        playerData = stateMachine.Player.Data.PlayerData;
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
        //stateMachine.Player.Controller.Move((direction * movementSpeed) * Time.deltaTime);
        stateMachine.Player.transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        Debug.Log("Move speed: " + movementSpeed);
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

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier * stateMachine.Player.moveSpeed; // �÷��̾��� ���ǵ� �����ֱ�(�������̳� ���������� ���� ���� �Ǿ��� ��)
        Debug.Log("Movement Speed: " + stateMachine.MovementSpeed);
        Debug.Log("movementSpeedModifier: " + stateMachine.MovementSpeedModifier);
        return movementSpeed;
    }

    protected virtual void OnAttack()
    {
        stateMachine.ChangeState(stateMachine.AttackState);
    }
}
