using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelBoundsBehaviour : MonoBehaviour {

    [SerializeField] [Range(100f, 500f)] private float boundsRadius = 200f;
    [SerializeField] [Range(50f, 100f)] private float boundsTolerance = 50f;

    private PlayerBehaviour playerObject;
    private Image boundsWarningPanel;
    private Text boundsWarningText;

    public float BoundsRadius { get { return boundsRadius; } }

    private State currentState;
    private NormalState normalState;
    private WarningState warningState;
    private DangerState dangerState;

    private class NormalState : State
    {
        LevelBoundsBehaviour levelBounds;

        public NormalState(LevelBoundsBehaviour levelBounds)
        {
            this.levelBounds = levelBounds;
        }

        public override void Update()
        {
            levelBounds.boundsWarningPanel.color = Color.Lerp(levelBounds.boundsWarningPanel.color, new Color(0.7f, 0f, 0f, 0f), 0.1f);
            levelBounds.boundsWarningText.color = Color.Lerp(levelBounds.boundsWarningText.color, new Color(1f, 1f, 1f, 0f), 0.1f);

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
        LevelBoundsBehaviour levelBounds;

        public WarningState(LevelBoundsBehaviour levelBounds)
        {
            this.levelBounds = levelBounds;
        }

        public override void Update()
        {
            levelBounds.boundsWarningPanel.color = Color.Lerp(levelBounds.boundsWarningPanel.color, new Color(0.7f, 0f, 0f, 0.2f), 0.1f);

            levelBounds.boundsWarningText.color = Color.Lerp(levelBounds.boundsWarningText.color, new Color(1f, 1f, 1f, 1f), 0.1f);
            levelBounds.boundsWarningText.text = "WARNING!\nYou are leaving the asteroid belt\nTurn back now!\n" + (int)((levelBounds.boundsRadius + levelBounds.boundsTolerance) - levelBounds.playerObject.transform.position.magnitude);
            
            if (levelBounds.playerObject.transform.position.magnitude < levelBounds.boundsRadius)
                Exit(levelBounds.normalState);
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
        LevelBoundsBehaviour levelBounds;

        public DangerState(LevelBoundsBehaviour levelBounds)
        {
            this.levelBounds = levelBounds;
        }

        public override void Update()
        {
            levelBounds.boundsWarningPanel.color = Color.Lerp(levelBounds.boundsWarningPanel.color, new Color(0.8f, 0f, 0f, 0.5f), 0.1f);

            levelBounds.boundsWarningText.color = Color.Lerp(levelBounds.boundsWarningText.color, new Color(1f, 1f, 1f, 1f), 0.1f);
            levelBounds.boundsWarningText.text = "WARNING!\nYou are leaving the asteroid belt\nTurn back now!\nDANGER!";

            levelBounds.playerObject.DealDamage(0.5f);

            if (levelBounds.playerObject.transform.position.magnitude < levelBounds.boundsRadius + levelBounds.boundsTolerance)
                Exit(levelBounds.normalState);
        }

        public override void Exit(State exitState)
        {
            levelBounds.currentState = exitState;
            levelBounds.currentState.Entry();
        }
    }

    void Awake()
    {
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

            // Enter the notmal state
            currentState = normalState;
            currentState.Entry();
        }
        else
            gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // If the player object extist, update the current state
        currentState.Update();
	}
}
