using UnityEngine;
using System.Collections;

public class EMPPowerup : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private int maxEMPs = 10;          //Max amount of EMPs the player can have
    [SerializeField] [Range(0, 10)] private int nmbrOfEMPs = 1;         //Number of Shields the player starts with
    


    [SerializeField] private Transform player;                          //Transform for the parent of the emp i.e the player ship
    [SerializeField] private GameObject EMPs;                         //Shield prefab gameobject

    void Update()
    {
        //If button Left Shift is presed we check if we have any shield left, if we do then we create a shield
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (nmbrOfEMPs > 0)
            {
                StartCoroutine(EMP(0.1f));
                nmbrOfEMPs -= 1;
            }
            else
            {
                Debug.Log("No EMPs left");
            }
        }
    }

    IEnumerator EMP(float waitTime)
    {
        GameObject go = Instantiate(EMPs, player.transform.position, player.rotation, player) as GameObject;

        yield return new WaitForSeconds(waitTime);

        Destroy(go);
    }

    //Increases the amount of EMPs the player has, is accessible outside of script
    public void IncreaseNmbrOfEMPs(int nmbr)
    {
        if (nmbrOfEMPs < maxEMPs)
        {
            nmbrOfEMPs += nmbr;
        }
        else
        {
            Debug.Log("Max amount of EMPs reached");
        }
    }   
}