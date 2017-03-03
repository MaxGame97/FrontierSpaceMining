using UnityEngine;
using System.Collections;

public class _PanningTransform : MonoBehaviour {
    
    [SerializeField] private Vector2 speed;

	// Update is called once per frame
	void Update () {
        transform.Translate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, 0f);
	}
}
