using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    SoldierClass unit;
    [SerializeField] CommandInvoker invoker;
    [SerializeField] TileMap map;

    [Header("Debug - do not touch")]
    [SerializeField]
    Vector2Int commandedPosition;

    bool canCommand;

    private void Awake()
    {
        invoker.onDequeueEnd += ToggleCommand; //toggle command gets triggered at the event the invoker broadcasts when it finishes the command queue
        map.OnTileClick += TryQueueMove;
    }

    void Start()
    {
        canCommand = true; //the player can command at the start of the game
    }
    
    void Update()
    {
        if (canCommand)
        {
            // SPECIAL ATTACK
            if (Input.GetKeyDown(KeyCode.S) && unit.CheckIfHero())
            {
                IAction specialAttack = new SpecialAttackAction(unit);
                invoker.AddAction(specialAttack);
                Debug.Log("Special attack queued");
            }

            //// START COMMAND DEQUEUE
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    Debug.Log("Dequeue");
            //    canCommand = false; //player can't interact while the invoker is busy
            //    unit.ResetStats();
            //    invoker.StartCommands();
            //}
        }

    }

    //restores player input once the invoker finishes
    private void ToggleCommand() 
    {
        Debug.Log("End command list");
        invoker.EndCommands();
        canCommand = true;
    }

    //method to encapsulate the unit field
    public void SetUnit(SoldierClass newUnit)
    {
        unit = newUnit;
        commandedPosition = new Vector2Int(unit.unitMovement.tileX, unit.unitMovement.tileY);
    }

    //queues move ONLY if the unit has enough mp to travel
    private void TryQueueMove(int x, int y)

    {
        if (canCommand)
        {
            
            if (map.CheckPathPrice(commandedPosition.x, commandedPosition.y, x, y, unit.currentMP, unit.unitMovement) != null)
            {
                List<Node> path = map.CheckPathPrice(commandedPosition.x, commandedPosition.y, x, y, unit.currentMP, unit.unitMovement);
                int pathPrice = (int)map.ReturnPathPrice(commandedPosition.x, commandedPosition.y, x, y, unit.unitMovement);

                unit.currentMP -= pathPrice;

                IAction moveAction = new MoveAction(unit, path);
                invoker.AddAction(moveAction);
                Debug.Log("Move queued to " + x + ", " + y);

                commandedPosition = new Vector2Int(x, y);

            }
            else Debug.Log("Not enough MP, you have: " + unit.currentMP + ", and it costs: " + map.ReturnPathPrice(commandedPosition.x, commandedPosition.y, x, y, unit.unitMovement));

            
        }
    }

    //checks if unit has AP to attack, if the chosen target is an enemy and if its located close enough to attack
    public void TryQueueAttack(int x, int y, SoldierClass otherUnit)
    {
        Debug.Log("Try attack.");
        if (unit.currentAP > 0)
        {
            if (otherUnit.IsEnemy != unit.IsEnemy) //checks if its an opponent and not a teammate
            {
                if (Mathf.Abs(otherUnit.unitMovement.tileX - commandedPosition.x) == 1) //left and right
                {
                    if (Mathf.Abs(otherUnit.unitMovement.tileY - commandedPosition.y) == 0)
                    {
                        QueueAttack(otherUnit);
                    }
                }
                else if (Mathf.Abs(otherUnit.unitMovement.tileY - commandedPosition.y) == 1) //up and down
                {
                    if (Mathf.Abs(otherUnit.unitMovement.tileX - commandedPosition.x) == 0)
                    {
                        QueueAttack(otherUnit);
                    }
                }
            }
        }
    }

    private void QueueAttack(SoldierClass otherUnit)
    {
        unit.currentAP -= 1;

        IAction attackAction = new AttackAction(unit, otherUnit);
        invoker.AddAction(attackAction);
        Debug.Log("Attack queued");
    }

    public void DisableInput()
    {
        map.OnTileClick -= TryQueueMove;
    }

    public void FinishTurn()
    {
        Debug.Log("Dequeue");
        canCommand = false; //player can't interact while the invoker is busy
        unit.ResetStats();
        invoker.StartCommands();
    }
}
