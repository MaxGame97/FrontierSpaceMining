using UnityEngine;
using System.Collections;

public class ShieldPowerup : MonoBehaviour {

    [SerializeField] [Range(0, 10)] private int nmbrOfShields = 1;      //Number of Shields the player starts with
    [SerializeField] [Range(1, 15)] private float shieldTime = 5;       //Duration time of the shield

    [SerializeField] private Transform player;                          //Transform for the parent of the shield i.e the player ship
    [SerializeField] private GameObject Shield;                         //Shield prefab gameobject


	// Update is called once per frame
	void Update () {
        //If button Left Shift is presed we check f we have any shield left, if we do then we c
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (nmbrOfShields > 0)
            {
                StartCoroutine(Shielded(shieldTime));
                nmbrOfShields -= 1;
            }
            else
            {
                Debug.Log("No shield powerups left");
            }
        }
	}
    IEnumerator Shielded(float waitTime)
    {
        GameObject go = Instantiate(Shield, player.transform.position, player.rotation, player) as GameObject;
        //go.transform.SetParent(GameObject.Find("Player").transform);

        yield return new WaitForSeconds(waitTime);

        Destroy(go);
    }
    
}
