using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : IAction
{
    SoldierClass currentUnit;

    public MoveAction(SoldierClass unit)
    {
        currentUnit = unit;
    }


    public void Execute()
    {
        currentUnit.Move();
    }

}
