using UnityEngine;
using System.Collections;

public class DieAfterTime : MonoBehaviour {

    [SerializeField]
    float deathTimer;
    private float count;

    private void Update()
    {
        count += Time.deltaTime;
        if (count >= deathTimer)
        {
            transform.parent.parent.parent.GetComponentInChildren<ConveyorBeltBehaviour>().movingObjectsList.Remove(gameObject.transform);
            Destroy(gameObject);
        }
    }
}
