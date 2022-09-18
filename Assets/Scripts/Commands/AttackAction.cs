using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : IAction
{
    SoldierClass currentUnit;

    public AttackAction(SoldierClass unit)
    {
        currentUnit = unit;
    }


    public void Execute()
    {
        currentUnit.Attack();
    }
}
