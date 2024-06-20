using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterStats
{
    void Start()
    {
        attackPower = 8;
        attackSpeed = 1.2f;
        experience = 50;
    }

}
