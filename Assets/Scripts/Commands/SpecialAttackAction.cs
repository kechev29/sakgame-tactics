using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackAction : IAction
{
    SoldierClass currentUnit;

    public SpecialAttackAction(SoldierClass unit)
    {
        currentUnit = unit;
    }

    public void Execute()
    {
        currentUnit.SpecialAttack();
    }

}
