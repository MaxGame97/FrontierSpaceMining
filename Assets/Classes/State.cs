using UnityEngine;
using System.Collections;

public class State {
    public virtual void Entry()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void Exit(State exitState)
    {
    }
}
