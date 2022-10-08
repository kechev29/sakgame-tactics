using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : IAction
{
    SoldierClass currentUnit;
    List<Node> path;

    public MoveAction(SoldierClass unit, List<Node> targetPath)
    {
        currentUnit = unit;
        path = targetPath;
    }


    public void Execute()
    {
        currentUnit.Move(path);
    }

}
