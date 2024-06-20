using UnityEngine;


[CreateAssetMenu(fileName = "RangedAttackSO", menuName = "AttackBaldHair/Attacks/Ranged", order = 1)]
public class RangedAttackSO : AttackSO
{
    [Header("Ranged Attack Data")]
    public string bulletNameTag;
    public float duration;
    public float spread;
    public int numberofProjectilesPerShot;
    public float multipleProjectilesAngle;
    public Color projectileColor;
}
