using UnityEngine;
using System.Collections;

public class MenuButtons : MonoBehaviour {

	public void Load(int index)
    {
        GameObject.Find("Global Game Controller").GetComponent<GlobalGameControllerBehaviour>().Load(index);
    }
}
