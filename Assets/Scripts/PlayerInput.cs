using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] SoldierClass unit;
    [SerializeField] CommandInvoker invoker;

    bool canCommand;

    private void Awake()
    {
        invoker.onDequeueEnd += ToggleCommand;
    }

    // Start is called before the first frame update
    void Start()
    {
        canCommand = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCommand)
        {
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Dequeue");
                invoker.StartCommands();
                canCommand = false;
            }
        }

    }

    private void ToggleCommand()
    {
        Debug.Log("End command list");
        invoker.EndCommands();
        canCommand = true;
    }

}
