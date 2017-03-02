using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class insideBehaviour : MonoBehaviour
{
    private Color startColor;

    private State currentState;
    private NormalState normalState;
    private HiddenState hiddenState;

    private class NormalState : State
    {
        insideBehaviour a;

        SpriteRenderer spriteRenderer;

        public NormalState(insideBehaviour a)
        {
            this.a = a;

            spriteRenderer = a.GetComponent<SpriteRenderer>();
        }

        public override void Update()
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(a.startColor.r, a.startColor.g, a.startColor.b, 1f), 0.1f);
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Exit(a.hiddenState);
            }
        }

        public override void Exit(State exitState)
        {
            a.currentState = exitState;
            a.currentState.Entry();
        }
    }

    private class HiddenState : State
    {
        insideBehaviour a;

        SpriteRenderer spriteRenderer;

        public HiddenState(insideBehaviour a)
        {
            this.a = a;

            spriteRenderer = a.GetComponent<SpriteRenderer>();
        }

        public override void Update()
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(a.startColor.r, a.startColor.g, a.startColor.b, 0f), 0.1f);
        }

        public override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Exit(a.normalState);
            }
        }

        public override void Exit(State exitState)
        {
            a.currentState = exitState;
            a.currentState.Entry();
        }
    }

    void Awake()
    {
        normalState = new NormalState(this);
        hiddenState = new HiddenState(this);
    }

    void Start()
    {
        startColor = GetComponent<SpriteRenderer>().color;

        currentState = normalState;
        currentState.Entry();
    }

    void Update()
    {
        currentState.Update();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OnTriggerEnter2D(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        currentState.OnTriggerExit2D(collision);
    }
}

