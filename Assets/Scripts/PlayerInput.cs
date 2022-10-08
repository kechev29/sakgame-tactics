using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    SoldierClass unit;
    [SerializeField] CommandInvoker invoker;

    bool canCommand;

    private void Awake()
    {
        invoker.onDequeueEnd += ToggleCommand; //toggle command gets triggered at the event the invoker broadcasts when it finishes the command queue
    }

    void Start()
    {
        canCommand = true; //the player can command at the start of the game
    }
    
    void Update()
    {
        if (canCommand)
        {
            // COMMAND INPUTS
            if (Input.GetKeyDown(KeyCode.A))
            {
                IAction attackAction = new AttackAction(unit);
                invoker.AddAction(attackAction);
                Debug.Log("Attack queued");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                IAction moveAction = new MoveAction(unit);
                invoker.AddAction(moveAction);
                Debug.Log("Move queued");
            }
            // START COMMAND DEQUEUE
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Dequeue");
                invoker.StartCommands();
                canCommand = false; //player can't interact while the invoker is busy
            }
        }

    }

    //returns player input once the invoker finishes
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
    }
}
