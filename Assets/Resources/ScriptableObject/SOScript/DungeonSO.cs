using UnityEngine;


public enum DungeonType
{
    Coin,
    Exp,
    Random
}

[CreateAssetMenu(fileName = "DungeonSO", menuName = "AttackBaldHair/Dungeon")]
public class DungeonSO : ScriptableObject
{
    [Header("Dungeon Info")]
    public int dungeonLv;
    public DungeonType Type;
    public int reward;
    public int defaultEnemyNum;
}
