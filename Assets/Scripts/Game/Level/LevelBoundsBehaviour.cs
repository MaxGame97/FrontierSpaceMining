using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBoundsBehaviour : MonoBehaviour {

    [SerializeField] [Range(100f, 500f)] private float boundsRadius = 200f;     // The bounds radius
    [SerializeField] [Range(50f, 100f)] private float boundsTolerance = 50f;    // The extra tolerance radius before the player starts taking damage

    private PlayerBehaviour playerObject;   // Reference to the player object
    private Image boundsWarningPanel;       // The bounds warning panel
    private Text boundsWarningText;         // The bounds warning text

    public float BoundsRadius { get { return boundsRadius; } }

    private State currentState;         // The bounds warning behaviour's current state
    private NormalState normalState;    // The state used when the player is within the level bounds
    private WarningState warningState;  // The state used when the player is about to leave the level bounds
    private DangerState dangerState;    // The state used when the player is outsied the level bounds

    private class NormalState : State
    {
        // Reference to the level bounds script
        LevelBoundsBehaviour levelBounds;

        public NormalState(LevelBoundsBehaviour levelBounds)
        {
            this.levelBounds = levelBounds;
        }

        public override void Update()
        {
            // Lerp the color of the bounds warning panel and text to be transparent
            levelBounds.boundsWarningPanel.color = Color.Lerp(levelBounds.boundsWarningPanel.color, new Color(0.7f, 0f, 0f, 0f), 0.1f);
            levelBounds.boundsWarningText.color = Color.Lerp(levelBounds.boundsWarningText.color, new Color(1f, 1f, 1f, 0f), 0.1f);

            // If the player is outside the bounds radius, exit to the warning state
            if (levelBounds.playerObject.transform.position.magnitude >= levelBounds.boundsRadius)
                Exit(levelBounds.warningState);
        }

        public override void Exit(State exitState)
        {
            levelBounds.currentState = exitState;
            levelBounds.currentState.Entry();
        }
    }

    private class WarningState : State
    {
        // Reference to the level bounds script
        LevelBoundsBehaviour levelBounds;

        public WarningState(LevelBoundsBehaviour levelBounds)
        {
            this.levelBounds = levelBounds;
        }

        public override void Update()
        {
            // Lerp the color of the bounds warning panel and text to be visible (semi-opaque)
            levelBounds.boundsWarningPanel.color = Color.Lerp(levelBounds.boundsWarningPanel.color, new Color(0.7f, 0f, 0f, 0.2f), 0.1f);
            levelBounds.boundsWarningText.color = Color.Lerp(levelBounds.boundsWarningText.color, new Color(1f, 1f, 1f, 1f), 0.1f);

            // Update the bounds warning text to indicate how far the player is able to move before starting to take damage
            levelBounds.boundsWarningText.text = "WARNING!\nYou are leaving the asteroid belt\nTurn back now!\n" + (int)((levelBounds.boundsRadius + levelBounds.boundsTolerance) - levelBounds.playerObject.transform.position.magnitude);
            
            // If the player has returned within the level bounds, exit to the normal state
            if (levelBounds.playerObject.transform.position.magnitude < levelBounds.boundsRadius)
                Exit(levelBounds.normalState);
            // Else it the player has left the extended level bounds, exit to the danger state
            else if (levelBounds.playerObject.transform.position.magnitude > levelBounds.boundsRadius + levelBounds.boundsTolerance)
            {
                Exit(levelBounds.dangerState);
            }
        }

        public override void Exit(State exitState)
        {
            levelBounds.currentState = exitState;
            levelBounds.currentState.Entry();
        }
    }

    private class DangerState : State
    {
        // Reference to the level bounds script
        LevelBoundsBehaviour levelBounds;

        public DangerState(LevelBoundsBehaviour levelBounds)
        {
            this.levelBounds = levelBounds;
        }

        public override void Update()
        {
            // Lerp the color of the bounds warning panel and text to be visible (semi-opaque), should this be neccesary (you never know)
            levelBounds.boundsWarningPanel.color = Color.Lerp(levelBounds.boundsWarningPanel.color, new Color(0.8f, 0f, 0f, 0.5f), 0.1f);
            levelBounds.boundsWarningText.color = Color.Lerp(levelBounds.boundsWarningText.color, new Color(1f, 1f, 1f, 1f), 0.1f);

            // Update the bounds warning text to indicate that the player is in the danger zone
            levelBounds.boundsWarningText.text = "WARNING!\nYou are leaving the asteroid belt\nTurn back now!\nDANGER!";

            // Deal 50 damage to the player each second
            levelBounds.playerObject.DealDamage(50f * Time.deltaTime);

            // If the player has returned within the extended level bounds, exit to the warning state
            if (levelBounds.playerObject.transform.position.magnitude < levelBounds.boundsRadius + levelBounds.boundsTolerance)
                Exit(levelBounds.warningState);
        }

        public override void Exit(State exitState)
        {
            levelBounds.currentState = exitState;
            levelBounds.currentState.Entry();
        }
    }

    void Awake()
    {
        // Instantiate the states

        normalState = new NormalState(this);
        warningState = new WarningState(this);
        dangerState = new DangerState(this);
    }

    // Use this for initialization
    void Start () {
        // If the player object exists
        if (GameObject.Find("Player") != null)
        {
            // Get the player object's player behaviour component
            playerObject = GameObject.Find("Player").GetComponent<PlayerBehaviour>();

            // Get the bounds warning objects
            boundsWarningPanel = GameObject.Find("Bounds Warning Panel").GetComponent<Image>();
            boundsWarningText = GameObject.Find("Bounds Warning Text").GetComponent<Text>();

            // Enter the normal state
            currentState = normalState;
            currentState.Entry();
        }
        // If the player object does not exist, deactivate this object
        else
            gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // If the player object extist, update the current state
        currentState.Update();
	}
}
