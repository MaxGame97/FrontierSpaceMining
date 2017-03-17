using UnityEngine;
using System.Collections;

public class _LaserFenceAreaEffector : MonoBehaviour {

    [SerializeField] [Range(0f, 360f)] private float relativeAngle = 0f;

	// Use this for initialization
	void Start () {
        float z = transform.eulerAngles.z;

        GetComponent<AreaEffector2D>().forceAngle = z - relativeAngle;
	}
}
