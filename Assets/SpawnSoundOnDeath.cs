using UnityEngine;
using System.Collections;

public class SpawnSoundOnDeath : MonoBehaviour {

    public GameObject sound;

    private void OnDestroy()
    {
        Instantiate(sound, new Vector3(0, 0, 0) , Quaternion.identity ,transform.parent );
    }

}
