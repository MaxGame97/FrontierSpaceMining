using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlackHole : MonoBehaviour
{
    [SerializeField] private GameObject areaEffectorPrefab;

    [SerializeField] [Range(1, 3)] private int areaEffectorLayers = 3;

    [SerializeField] [Range(5f, 18f)] private float areaEffectorRadius = 15f;
    [SerializeField] [Range(0f, 1f)] private float steepness = 0.5f;
    [SerializeField] [Range(-20f, 20f)] private float areaEffectorForce = 3f;
    
	// Use this for initialization
	void Start ()
    {
        SpawnEffectors();

        GameObject blackHolePull = transform.FindChild("BlackHolePull").gameObject;
        GameObject blackHoldPush = transform.FindChild("BlackHolePush").gameObject;

        blackHolePull.transform.localScale = new Vector3(areaEffectorRadius * 2f * (areaEffectorLayers + 1), areaEffectorRadius * 2f * (areaEffectorLayers + 1), 1f);
        blackHolePull.GetComponent<PointEffector2D>().forceMagnitude = (-Mathf.Abs(areaEffectorForce) * Mathf.PI) * steepness;

        blackHoldPush.transform.localScale = new Vector3(areaEffectorRadius * 2f, areaEffectorRadius * 2f, 1f);
        blackHoldPush.GetComponent<PointEffector2D>().forceMagnitude = (Mathf.Abs(areaEffectorForce) * Mathf.PI * 2f) * steepness;
    }

    void SpawnEffectors()
    {
        for(int i = 2; i < areaEffectorLayers + 2; i++)
        {
            float circumference = Mathf.PI * ((areaEffectorRadius * 2 * i) * 2f);

            int amount = (int)(circumference / (areaEffectorRadius * 2f));

            for (int j = 0; j < amount; j++)
            {
                float angle = (360f / amount) * j;
                Vector3 pos = new Vector3();
                
                pos.x = Mathf.Sin(Mathf.Deg2Rad * angle) * ((areaEffectorRadius * 2 * i) - areaEffectorRadius);
                pos.y = Mathf.Cos(Mathf.Deg2Rad * angle) * ((areaEffectorRadius * 2 * i) - areaEffectorRadius);
                pos.z = transform.position.z;
                
                GameObject effector = (GameObject)Instantiate(areaEffectorPrefab, transform.position + pos, new Quaternion());

                effector.transform.SetParent(transform);
                effector.transform.localScale = new Vector3(areaEffectorRadius, areaEffectorRadius, 0f);

                effector.GetComponent<AreaEffector2D>().forceAngle = 180f - angle;
                effector.GetComponent<AreaEffector2D>().forceMagnitude = areaEffectorForce;
            }
        }
    }
}