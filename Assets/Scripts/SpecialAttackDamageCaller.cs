using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpecialAttackDamageCaller : MonoBehaviour
{
    public event Action<SoldierClass, int> OnSpecialAttack;

    public void BroadcastSpecialAttack(SoldierClass unit, int damage)
    {
        OnSpecialAttack?.Invoke(unit, damage);
    }

}
