using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CommandInvoker : MonoBehaviour //needed the monobehaviour so i can use coroutines
{
    //queue so actions are handed out in the order they were selected (first in, first out)
    Queue<IAction> actionList = new Queue<IAction>();

    //event triggered when queue ends in order to return control to player
    public event Action onDequeueEnd;


    // - ADD ACTION TO QUEUE -
    public void AddAction(IAction newAction)
    {
        actionList.Enqueue(newAction);
    }

    // - DEQUEUE COROUTINE METHODS -
    public void StartCommands()
    {
        StartCoroutine(DequeueActions());
    }
    public void EndCommands()
    {
        StopCoroutine(DequeueActions());
    }

    // - THE ACTUAL COROUTINE -
    public IEnumerator DequeueActions()
    {
        while(actionList.Count > 0)
        {
            IAction current = actionList.Dequeue();
            current.Execute();
            yield return new WaitForSeconds(1f);
        }

        if (onDequeueEnd != null) onDequeueEnd();
    }

}
