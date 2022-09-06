using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit Data")]
public class UnitsStats : ScriptableObject
{
    [SerializeField] private string unitName;
    [SerializeField] private string unitDescription;
    [SerializeField] private int attackPoints;
    [SerializeField] private int healthPoints;
    [SerializeField] private int defensePoints;
    [SerializeField] private int movementPoints;
    [SerializeField] private int actionPoints;
    private int damage;

    public string UnitName { get { return unitName; } }
    public string UnitDescription { get { return unitDescription; } }
    public int AttackPoints { get { return attackPoints; } }
    public int HealthPoints { get { return healthPoints; } }
    public int DefensePoints { get { return defensePoints; } }
    public int MovementPoints { get { return movementPoints; } }
    public int ActionPoints { get { return actionPoints; } }

    public int Damage { get { return damage = attackPoints - defensePoints * Random.Range(0, 100); } }

    

}
