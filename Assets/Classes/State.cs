using UnityEngine;
using System.Collections;

public class State {
    public virtual void Entry()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
    }

    public virtual void Exit(State exitState)
    {
    }
}
