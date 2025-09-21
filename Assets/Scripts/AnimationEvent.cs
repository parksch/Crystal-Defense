using System.Collections.Generic;
using System;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    List<Action> actions;

    public void Set()
    {
        if (actions == null)
        {
            actions = new List<Action>();
        }

        actions.Clear();
    }

    public void AddAction(Action action)
    {
        actions.Add(action);
    }

    public void OnAction(int value)
    {
        actions[value]?.Invoke();
    }
}
