using System;
using UnityEngine;

[Serializable]
public class EnemyAnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string runParameterName = "Run";


    [SerializeField] private string attackParameterName = "@Attack";
    [SerializeField] private string baseAttackParameterName = "BaseAttack";

    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int RunParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int BaseAttackParameterHash { get; private set; }


    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        BaseAttackParameterHash = Animator.StringToHash(baseAttackParameterName);
    }
}
