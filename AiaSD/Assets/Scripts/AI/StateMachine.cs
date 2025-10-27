using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMacine : MonoBehaviour
{
    public State startingState;
    State current;

    void Awake()
    {
        current = startingState;
    }
    void Update()
    {
        if (current == null) return;
        State next = current.Run(gameObject);
        if (next != current)
        {
            current = next;
        }
    }
}