using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : IAction
{
    SoldierClass currentUnit;
    SoldierClass enemy;

    public AttackAction(SoldierClass unit, SoldierClass opponentUnit)
    {
        currentUnit = unit;
        enemy = opponentUnit;
    }


    public void Execute()
    {
        currentUnit.Attack(enemy);
    }
}
