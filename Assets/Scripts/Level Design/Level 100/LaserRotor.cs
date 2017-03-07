using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRotor : MonoBehaviour
{
   
    [SerializeField] private float rotationSpeed;


    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

}
