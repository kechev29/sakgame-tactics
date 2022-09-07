using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroClass : SoldierClass
{
    [SerializeField] private UnitsStats unitsStats;
 
    public override void Attack()
    {
        base.Attack();
    }
    public override void Move()
    {
        base.Move();
    }
    
   
}
