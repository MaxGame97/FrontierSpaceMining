using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour {

    [SerializeField] private float maxHealth; //The Player's maximum health
    [SerializeField] private float currentHealth; //The Player's current health

    [SerializeField] private Slider healthSlider;  //Reference to the UI healthbar

    [SerializeField] private PlayerMovement playerMovement;    //Reference 
    [SerializeField] private MiningLaser playerMining;
    [SerializeField] private ItemPickupBehaviour playerPickup;


    private bool isDead;

	void Awake () {

        
        currentHealth = maxHealth;  //Starting health of the player
        healthSlider.maxValue = maxHealth;  //Maximum health on the healthbar set
        healthSlider.value = maxHealth;     //Starting health of the player on the healthbar
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void TakeDamage(float amount)
    {

        currentHealth -= amount;    //Reduce current health with the amount of damage taken


        healthSlider.value = currentHealth;     //Change the slider's position to match the health of the player


        if(currentHealth <= 0 && !isDead) //If all health is lost and the isDead bool is not yet set to true, 
        {
            Death();    //the player Dies
        }
    }

    void Death()
    {

        isDead = true;  //Sets isDead to true, so that the function won't be called again.

        playerMovement.enabled = false; //Disables movement when dead
        playerMining.enabled = false;   //Disables mining when dead
        playerPickup.enabled = false;   //Disables pickups when dead
    }
}
