using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] SoldierClass[] playerUnits;
    [SerializeField] SoldierClass[] enemyUnits;
    [SerializeField] PlayerInput input;
    [SerializeField] CommandInvoker invoker;

    List<SoldierClass> allUnits = new List<SoldierClass>();
    SoldierClass currentUnit;
    int counter;

    private void Awake()
    {
        SetupUnitList();
        counter = 0;
        invoker.onDequeueEnd += NextUnit; //the unit changes once the invoker ends calling the commands
        
    }

    private void Start()
    {
        //PrintUnitList();
        currentUnit = allUnits[counter];
        SetUnit();
    }

    //fills the allUnits list with the player and enemy units, alternating between them (player1, then enemy1, then player2, etc.)
    private void SetupUnitList()
    {
        int longestUnitList = playerUnits.Length;
        if (enemyUnits.Length > longestUnitList) longestUnitList = enemyUnits.Length; 

        for (int i = 0; i < longestUnitList; i++)
        {
            if (i < playerUnits.Length)
            {
                allUnits.Add(playerUnits[i]);
            }
            if (i < enemyUnits.Length)
            {
                enemyUnits[i].IsEnemy = true;
                allUnits.Add(enemyUnits[i]);
            }
        }
    }

    //prints the gameObject name of every unit in the order its found in the list
    private void PrintUnitList()
    {
        for (int i = 0; i < allUnits.Count; i++)
        {
            Debug.Log(allUnits[i].gameObject.name + " is enemy: " + allUnits[i].IsEnemy);
        }
    }

    //changes the current unit for the player input controller
    private void SetUnit()
    {
        Debug.Log("Turn: " + currentUnit.name);

        input.SetUnit(currentUnit);
    }

    //picks the next unit once the previous one ends the command
    private void NextUnit()
    {
        counter += 1;
        if(allUnits[counter % allUnits.Count].isAlive == false)
        {
            SoldierClass unit = allUnits[counter % allUnits.Count];
            allUnits.Remove(unit);
        }

        if (allUnits.Count > 1 && CheckUnitList())
        {
            currentUnit = allUnits[counter % allUnits.Count]; //using % makes the list starts over once it reaches the end
            SetUnit();
        }
        else
        {
            Debug.Log("Game over");
            input.enabled = false;
        }
        
    }

    private bool CheckUnitList()
    {
        bool enemyFound = false;
        bool notEnemyFound = false;

        foreach (SoldierClass unit in allUnits)
        {
            if (unit.IsEnemy) enemyFound = true;
            else notEnemyFound = true;

            if (enemyFound && notEnemyFound) return true;
        }

        return false;
    }
}
