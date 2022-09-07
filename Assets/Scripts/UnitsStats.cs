using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Unit Data")]
public class UnitsStats : ScriptableObject
{
    [SerializeField] private int soldierAttackPoints;
    [SerializeField] private int soldierHealthPoints;
    [SerializeField] private int soldierDefensePoints;
    [SerializeField] private int soldierMovementPoints;
    [SerializeField] private int soldierActionPoints;
    [SerializeField] private int heroAttackPoints;
    [SerializeField] private int heroHealthPoints;
    [SerializeField] private int heroDefensePoints;
    [SerializeField] private int heroMovementPoints;
    [SerializeField] private int heroActionPoints;
    private int soldierDamage;
    public int SoldierAttackPoints { get { return soldierAttackPoints; } }
    public int SoldierHealthPoints { get { return soldierHealthPoints; } }
    public int SoldierDefensePoints { get { return soldierDefensePoints; } }
    public int SoldierMovementPoints { get { return soldierMovementPoints; } }
    public int SoldierActionPoints { get { return soldierActionPoints; } }
    public int SoldierDamage { get { return soldierDamage = soldierAttackPoints - soldierDefensePoints * Random.Range(0, 100); } }
    public int HeroAttackPoints { get { return heroAttackPoints; } }
    public int HeroHealthPoints { get { return heroHealthPoints; } }
    public int HeroDefensePoints {get { return heroDefensePoints; } }
    public int HeroMovementPoints { get { return heroMovementPoints; } }  
    public int HeroActionPoints { get { return heroActionPoints; } }   
    

}
