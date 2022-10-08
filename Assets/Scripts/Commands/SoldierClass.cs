using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoldierClass : MonoBehaviour
{
    [SerializeField] private UnitsStats unitsStats;
    [SerializeField] private TileMap map;
    [SerializeField] private SpecialAttackDamageCaller SAdamage;

    [Header("Debug - do not touch")]
    public int currentMP;
    public int currentAP;
    public int currentHP;

    bool isEnemy;
    public bool isAlive;

    [System.NonSerialized] public Unit unitMovement;

    

    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }

    private void Awake()
    {
        currentMP = unitsStats.SoldierMovementPoints;
        currentAP = unitsStats.SoldierActionPoints;
        currentHP = unitsStats.SoldierHealthPoints;

        isAlive = true;

        SAdamage.OnSpecialAttack += CheckIfDamagedBySA;

        unitMovement = GetComponent<Unit>();
    }

    public void Attack(SoldierClass enemy)
    {
        int damage = (int)(unitsStats.SoldierAttackPoints - enemy.ReturnDefense() * UnityEngine.Random.Range(0f, 1f));
        if (damage < 0) damage = 0;
        enemy.Damage(damage);

        Debug.Log(transform.gameObject.name + ": Attack " + enemy.name + " for " + damage + " damage.");
    }
    public void SpecialAttack()
    {
        int damage = (int)((unitsStats.SoldierAttackPoints * 2) * UnityEngine.Random.Range(0f, 1f));
        SAdamage.BroadcastSpecialAttack(this, damage);
        
        Debug.Log("Special Attack");
    }


    public void Move(List<Node> path)
    {
        unitMovement.currentPath = path;
        unitMovement.SetWalkable();
        //Debug.Log(transform.gameObject.name + ": Move");
    }

    public void ResetStats()
    {
        currentAP = unitsStats.SoldierActionPoints;
        currentMP = unitsStats.SoldierMovementPoints;
    }

    public int ReturnDefense()
    {
        return unitsStats.SoldierDefensePoints;
    }

    public bool CheckIfHero()
    {
        return unitsStats.IsHero;
    }

    public void Damage(int value)
    {
        currentHP -= value;
        if (currentHP <= 0)
        {
            currentHP = 0;
            Debug.Log(gameObject.name + " is dead.");
            isAlive = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }

    private void CheckIfDamagedBySA(SoldierClass sourceUnit, int damageAmount)
    {
        
        if(sourceUnit.isEnemy != this.isEnemy)
        {
            Debug.Log("Special attack check for " + gameObject.name);
            if (Mathf.Abs(this.unitMovement.tileX - sourceUnit.unitMovement.tileX) <= 2 && Mathf.Abs(this.unitMovement.tileX - sourceUnit.unitMovement.tileX) > 0) //left & right
            {
                if (Mathf.Abs(this.unitMovement.tileY - sourceUnit.unitMovement.tileY) == 0)
                {
                    this.Damage(damageAmount);
                    Debug.Log("Damaged " + gameObject.name + " for " + damageAmount + " points.");
                }
            } else if (Mathf.Abs(this.unitMovement.tileY - sourceUnit.unitMovement.tileY) <= 2 && Mathf.Abs(this.unitMovement.tileY - sourceUnit.unitMovement.tileY) > 0) //up & down
            {
                if (Mathf.Abs(this.unitMovement.tileX - sourceUnit.unitMovement.tileX) == 0)
                {
                    this.Damage(damageAmount);
                    Debug.Log("Damaged " + gameObject.name + " for " + damageAmount + " points.");
                }
            }
        }

    }
}
