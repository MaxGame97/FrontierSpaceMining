using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    [SerializeField] private float stunDuration = 5f;

    private PlayerMovement playerMovement;
    private ItemPickupBehaviour itemPickupBehaviour;
    private MiningLaser miningLaser;
    private PlayerHealth playerHealth;

    private State currentState;
    private AliveState aliveState;
    private StunnedState stunnedState;
    private DeadState deadState;

    private class AliveState : State
    {
        PlayerBehaviour player;

        public AliveState(PlayerBehaviour player)
        {
            this.player = player;
        }

        public override void Entry()
        {
            player.playerMovement.IsEnabled = true;
            player.itemPickupBehaviour.IsEnabled = true;
            player.miningLaser.IsEnabled = true;
            player.playerHealth.IsEnabled = true;
        }

        public override void Update()
        {

        }

        public override void Exit(State exitState)
        {
            player.currentState = exitState;
            player.currentState.Entry();
        }
    }

    private class StunnedState : State
    {
        PlayerBehaviour player;

        public StunnedState(PlayerBehaviour player)
        {
            this.player = player;
        }

        public override void Entry()
        {

        }

        public override void Update()
        {

        }

        public override void Exit(State exitState)
        {
            player.currentState = exitState;
            player.currentState.Entry();
        }
    }

    private class DeadState : State
    {
        PlayerBehaviour player;

        public DeadState(PlayerBehaviour player)
        {
            this.player = player;
        }

        public override void Entry()
        {
            player.playerMovement.IsEnabled = false;

            Rigidbody2D rigidbody = player.GetComponent<Rigidbody2D>();
            rigidbody.angularDrag = 0;
            rigidbody.drag = 0;

            player.itemPickupBehaviour.IsEnabled = false;
            player.miningLaser.IsEnabled = false;
            player.playerHealth.IsEnabled = false;
        }

        public override void Update()
        {

        }

        public override void Exit(State exitState)
        {
            player.currentState = exitState;
            player.currentState.Entry();
        }
    }

    void Awake()
    {
        aliveState = new AliveState(this);
        stunnedState = new StunnedState(this);
        deadState = new DeadState(this);
    }

	// Use this for initialization
	void Start () {
        playerMovement = GetComponent<PlayerMovement>();
        itemPickupBehaviour = GetComponent<ItemPickupBehaviour>();
        miningLaser = GetComponent<MiningLaser>();
        playerHealth = GetComponent<PlayerHealth>();

        currentState = aliveState;
        currentState.Entry();
	}
	
	void FixedUpdate () {
        currentState.Update();
	}

    public void StunPlayer()
    {
        if(currentState != stunnedState && currentState != deadState)
            currentState.Exit(stunnedState);
    }

    public void KillPlayer()
    {
        if (currentState != deadState)
            currentState.Exit(deadState);
    }
}
