using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierClass : MonoBehaviour
{
    [SerializeField] private UnitsStats unitsStats;
    bool isEnemy;

    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }

    public void Attack()
    {
        Debug.Log(transform.gameObject.name + ": Attack");
    }
    public void SpecialAttack()
    {
        if(unitsStats.IsHero)
        Debug.Log("Special Attack");
    }


    public void Move()
    {
        Debug.Log(transform.gameObject.name + ": Move");
    }



}
