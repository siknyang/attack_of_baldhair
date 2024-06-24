using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Player player;

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }
}
