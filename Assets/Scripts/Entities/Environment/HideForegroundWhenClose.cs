using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideForegroundWhenClose : MonoBehaviour
{
    private Color startColor;           // The starting color of the spriterenderer

    private State currentState;         // The current visibility state
    private NormalState normalState;    // State used when the foreground is opaque
    private HiddenState hiddenState;    // State used when the foreground is hidden

    private class NormalState : State
    {
        // Reference to the foreground
        HideForegroundWhenClose foreground;

        // Reference to the foreground's spriterenderer
        SpriteRenderer spriteRenderer;

        public NormalState(HideForegroundWhenClose foreground)
        {
            // Get the foreground and its spriterenderer

            this.foreground = foreground;

            spriteRenderer = foreground.GetComponent<SpriteRenderer>();
        }

        public override void Update()
        {
            // Lerp between the current color and the starting color (fully opaque)
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(foreground.startColor.r, foreground.startColor.g, foreground.startColor.b, 1f), 0.1f);
        }

        public override void OnTriggerEnter2D(Collider2D collision)
        {
            // If the foreground is triggering the player
            if (collision.gameObject.tag == "Player")
            {
                // Exit to the hidden state
                Exit(foreground.hiddenState);
            }
        }

        public override void Exit(State exitState)
        {
            foreground.currentState = exitState;
            foreground.currentState.Entry();
        }
    }

    private class HiddenState : State
    {
        // Reference to the foreground
        HideForegroundWhenClose foreground;

        // Reference to the foreground's spriterenderer
        SpriteRenderer spriteRenderer;

        public HiddenState(HideForegroundWhenClose foreground)
        {
            // Get the foreground and its spriterenderer

            this.foreground = foreground;

            spriteRenderer = foreground.GetComponent<SpriteRenderer>();
        }

        public override void Update()
        {
            // Lerp between the current color and the starting color (fully transparent)
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, new Color(foreground.startColor.r, foreground.startColor.g, foreground.startColor.b, 0f), 0.1f);
        }

        public override void OnTriggerExit2D(Collider2D collision)
        {
            // If the player is no longer triggering the foreground
            if (collision.gameObject.tag == "Player")
            {
                // Exit to the normal state
                Exit(foreground.normalState);
            }
        }

        public override void Exit(State exitState)
        {
            foreground.currentState = exitState;
            foreground.currentState.Entry();
        }
    }

    void Awake()
    {
        // Instantiate the visibility states

        normalState = new NormalState(this);
        hiddenState = new HiddenState(this);
    }

    void Start()
    {
        // Get the starting color
        startColor = GetComponent<SpriteRenderer>().color;

        // Change to and enter the normal state
        currentState = normalState;
        currentState.Entry();
    }

    void Update()
    {
        // Update the current visibility state
        currentState.Update();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger the current visibility state
        currentState.OnTriggerEnter2D(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Untrigger the current visibility state
        currentState.OnTriggerExit2D(collision);
    }
}

