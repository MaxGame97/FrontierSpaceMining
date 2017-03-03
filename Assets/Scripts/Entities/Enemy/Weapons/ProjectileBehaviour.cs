using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {


    [SerializeField] [Range(0f, 100f)] private float damage = 10;     // The damage inflicted by the bullet

    public float Damage { get { return damage; } }

}
