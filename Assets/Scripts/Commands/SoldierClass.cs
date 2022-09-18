using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierClass : MonoBehaviour
{
    [SerializeField] private UnitsStats unitsStats;


    public void Attack()
    {
        Debug.Log("Attack");
    }
    public void Move()
    {
        Debug.Log("Move");
    }
}
